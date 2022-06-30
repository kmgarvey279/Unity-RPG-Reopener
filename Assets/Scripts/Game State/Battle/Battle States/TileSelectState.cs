using System.Collections;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;
using StateMachineNamespace;
using UnityEngine.EventSystems;

[System.Serializable]
public class TileSelectState : BattleState
{
    [SerializeField] private BattleTimeline battleTimeline;
    private Combatant primaryTarget;
    private List<Combatant> targets;
    private CombatantType combatantType;
    [Header("Events")]
    [SerializeField] private SignalSender onCameraZoomOut;

    public override void OnEnter()
    {
        base.OnEnter();
        //set defaults
        primaryTarget = null;
        targets = new List<Combatant>();

        // onCameraZoomOut.Raise();

        //display tiles
        combatantType = CombatantType.Enemy;
        if(turnData.actionEvent.action.targetingType == TargetingType.TargetFriendly)
        {
            combatantType = CombatantType.Player;
        }
        gridManager.DisplaySelectableTargets(turnData.actionEvent.action, combatantType, turnData.combatant);
    }

    public override void StateUpdate()
    {
        if(Input.GetButtonDown("Select"))
        {
            if(targets.Count > 0) 
            {
                battleManager.SetTargets(primaryTarget, targets);
                //clear targets
                foreach(Combatant target in targets)
                {
                    target.Detarget();
                }
                stateMachine.ChangeState((int)BattleStateType.Execute); 
            }
            else 
            {
                Debug.Log("No target selected");
            }
        } 
        else if(Input.GetButtonDown("Cancel"))
        {
            foreach(Combatant target in targets)
            {
                target.Detarget();
                target.ToggleHPBar(false);
            }
            battleManager.CancelAction();
            stateMachine.ChangeState((int)BattleStateType.Menu);
        }
    }

    public override void StateFixedUpdate()
    {

    }

    public override void OnExit()
    {
        base.OnExit();
        //clear tiles
        gridManager.HideTiles();
        ResetTimelinePreview();
        
        battleManager.ToggleTargetSelect(false);
        battleTimeline.ClearAllTargetingPreviews();
    }

    public void OnSelectTarget(GameObject combatantObject)
    {
        if(targets.Count > 0)
        {
            //clear previous targets
            ResetTimelinePreview();
            foreach(Combatant target in targets)
            {
                target.Detarget();
                target.ToggleHPBar(false);
            } 
            targets.Clear();
        }

        primaryTarget = combatantObject.GetComponent<Combatant>();   
        targets.Add(primaryTarget);
        SpawnAOE();
    }

    private void SpawnAOE()
    {
        List<Tile> aoeTiles = gridManager.GetAOETiles(primaryTarget.tile, turnData.actionEvent.action, combatantType);
        gridManager.DisplayAOE(aoeTiles, combatantType);
        
        primaryTarget.Target();
        primaryTarget.ToggleHPBar(true);
        foreach(Combatant combatant in gridManager.GetTargetsInAOE(aoeTiles, combatantType))
        {
            if(!targets.Contains(combatant))
            {
                targets.Add(combatant);
                combatant.Target();
                combatant.ToggleHPBar(true);
            }
        }
        battleManager.DisplayTargetPreview(targets);

        foreach(ActionEffect actionEffect in turnData.actionEvent.action.actionEffects)
        {
            if(actionEffect is ActionEffectKnockback)
            {
                ActionEffectKnockback knockback = (ActionEffectKnockback)actionEffect;
                List<List<Tile>> knockbackPaths = new List<List<Tile>>();
                foreach(Combatant target in targets)
                {
                    Vector2Int knockbackDestination = knockback.GetKnockbackDestination(target.tile);
                    Tile newTile = gridManager.GetTileArray(CombatantType.Enemy)[knockbackDestination.x, knockbackDestination.y];
                    if(newTile != target.tile)
                    {
                        knockbackPaths.Add(gridManager.GetPath(target.tile, newTile, CombatantType.Enemy));
                    }
                }
                gridManager.DisplayPaths(knockbackPaths, CombatantType.Enemy);
                break;
            }
            if(actionEffect is ActionEffectTurnModifier)
            {
                ActionEffectTurnModifier turnEffect = (ActionEffectTurnModifier)actionEffect;
                foreach(Combatant combatant in targets)
                {
                   combatant.ApplyTurnModifier(turnEffect.turnModifier); 
                }
                // battleManager.DisplayTimelineChanges(tempList);
                battleManager.DisplayTimelinePreview(targets);
            }            
        }
    }

    private void ResetTimelinePreview()
    {    
        foreach(ActionEffect actionEffect in turnData.actionEvent.action.actionEffects)
        {    
            if(actionEffect is ActionEffectTurnModifier)
            {
                ActionEffectTurnModifier turnEffect = (ActionEffectTurnModifier)actionEffect;
                foreach(Combatant target in targets)
                {
                    target.ApplyTurnModifier(-turnEffect.turnModifier);
                }
                battleManager.ClearTimelinePreview();
            }
        }
    }
    
}
