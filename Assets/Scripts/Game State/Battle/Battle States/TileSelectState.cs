using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using StateMachineNamespace;
using UnityEngine.EventSystems;

[System.Serializable]
public class TileSelectState : BattleState
{
    [SerializeField] private GridManager gridManager;

    private Tile selectedTile;
    private List<Combatant> selectedTargets = new List<Combatant>();
    private TargetType targetType;
    private bool flip;
    [Header("Events")]
    [SerializeField] private SignalSender onCameraZoomOut;

    public override void OnEnter()
    {
        base.OnEnter();

        onCameraZoomOut.Raise();
        //display tiles
        targetType = TargetType.TargetEnemy;
        flip = false;
        if(turnData.action.targetFriendly)
        {
            targetType = TargetType.TargetPlayer;
        }

        if(turnData.action.isFixedTarget)
        {
            gridManager.DisplaySelectableTargets(turnData.combatant.tile.x, targetType, turnData.action.isMelee);
        }
        else if(turnData.action.isFixedAOE)
        {
            selectedTargets = gridManager.DisplayFixedAOE(turnData.combatant.tile, turnData.action, targetType, flip);
        }
        else
        {
            gridManager.DisplaySelectableTiles(targetType);
        }
    }

    public override void StateUpdate()
    {
        if(Input.GetButtonDown("Select"))
        {
            if(selectedTargets.Count > 0 || turnData.action.actionType == ActionType.Move) 
            {
                battleManager.SetTargets(selectedTile, selectedTargets, targetType);
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
            stateMachine.ChangeState((int)BattleStateType.Menu);
        }
        else if(Input.GetButtonDown("Switch"))
        {
            flip = !flip;
            if(turnData.action.aoes.Count > 0)
            {
                selectedTargets = gridManager.DisplayAOE(selectedTile, turnData.action, targetType, flip);
            }
        }
    }

    public override void StateFixedUpdate()
    {

    }

    public override void OnExit()
    {
        base.OnExit();
        //clear tiles
        selectedTile = null;
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
        if(turnData.action.aoes.Count > 0)
        {
            selectedTargets = gridManager.DisplayAOE(selectedTile, turnData.action, targetType, flip);
        }
        if(turnData.action.actionType == ActionType.Move)
        {
            List<Tile> path = gridManager.GetPath(turnData.combatant.tile, selectedTile, TargetType.TargetPlayer);
            gridManager.DisplayPath(path); 
        } 
    }
}
