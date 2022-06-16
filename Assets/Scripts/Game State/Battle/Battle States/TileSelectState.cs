using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using StateMachineNamespace;
using UnityEngine.EventSystems;

[System.Serializable]
public class TileSelectState : BattleState
{
    private Tile selectedTile;
    private List<Combatant> selectedTargets;
    private CombatantType combatantType;
    [Header("Events")]
    [SerializeField] private SignalSender onCameraZoomOut;

    public override void OnEnter()
    {
        base.OnEnter();
        //set defaults
        selectedTile = null;
        selectedTargets = new List<Combatant>();
        // onCameraZoomOut.Raise();
        //display tiles
        combatantType = CombatantType.Enemy;
        if(turnData.actionEvent.action.targetingType == TargetingType.TargetFriendly)
        {
            combatantType = CombatantType.Player;
        }

        if(turnData.actionEvent.action.isFixedTarget)
        {
            gridManager.DisplaySelectableTargets(turnData.actionEvent.action, combatantType, turnData.combatant.tile);
        }
        else
        {   
            gridManager.DisplaySelectableTiles(turnData.actionEvent.action, combatantType, turnData.combatant.tile);
            if(turnData.actionEvent.action.isFixedAOE)
            {
                SpawnAOEs();
            }
        }
    }

    public override void StateUpdate()
    {
        if(Input.GetButtonDown("Select"))
        {
            if(selectedTargets.Count > 0 || turnData.actionEvent.action.actionType == ActionType.Move) 
            {
                battleManager.SetTargets(selectedTile, selectedTargets);
                stateMachine.ChangeState((int)BattleStateType.Execute); 
            }
            else 
            {
                Debug.Log("No target selected");
            }
        } 
        else if(Input.GetButtonDown("Cancel"))
        {
            battleManager.CancelAction();
            foreach(Combatant combatant in selectedTargets)
            {
                combatant.ToggleHPBar(false);
            }
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
    }

    public void OnSelectTile(GameObject tileObject)
    {     
        selectedTile = tileObject.GetComponent<Tile>();     
        // selectedTargets = gridManager.GetTargetsInRange(selectedTile, turnData.action.aoe, turnData.action.targetFriendly, turnData.action.targetHostile);   
        // if(turnData.action.knockback)
        // {
        //     Vector2 direction = (selectedTile.transform.position - turnData.combatant.tile.transform.position).normalized;
        //     List<Tile> path = new List<Tile>();
            // if(selectedTile.occupier && selectedTile.occupier is EnemyCombatant)
            //     path = gridManager.GetRow(selectedTile, direction);
            // gridManager.DisplayPath(path);
        // } 
        if(turnData.actionEvent.action.aoes.Count > 0)
        {
            SpawnAOEs();
        }
        if(turnData.actionEvent.action.actionType == ActionType.Move)
        {
            List<Tile> path = gridManager.GetPath(turnData.combatant.tile, selectedTile, CombatantType.Player);
            gridManager.DisplayPaths(new List<List<Tile>>(){path}, CombatantType.Player); 
        } 
    }

    private void SpawnAOEs()
    {
        if(selectedTargets.Count > 0)
        {
            foreach(Combatant target in selectedTargets)
            {
                target.ToggleHPBar(false);
            }
        }
        List<Tile> aoeTiles = gridManager.GetAOETiles(selectedTile, turnData.actionEvent.action, combatantType);
        gridManager.DisplayAOE(aoeTiles, combatantType);
        
        selectedTargets = gridManager.GetTargetsInAOE(aoeTiles, combatantType);
        foreach(Combatant target in selectedTargets)
        {
            target.ToggleHPBar(true);
        }

        foreach(ActionEffectTrigger actionEffectTrigger in turnData.actionEvent.action.actionEffectTriggers)
        {
            if(actionEffectTrigger.actionEffect is ActionEffectKnockback)
            {
                ActionEffectKnockback knockback = (ActionEffectKnockback)actionEffectTrigger.actionEffect;
                List<List<Tile>> knockbackPaths = new List<List<Tile>>();
                foreach(Combatant target in selectedTargets)
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
        }
    }
    
}
