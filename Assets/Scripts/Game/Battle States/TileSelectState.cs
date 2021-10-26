using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using StateMachineNamespace;
using UnityEngine.EventSystems;

[System.Serializable]
public class TileSelectState : BattleState
{
    [SerializeField] private GridManager gridManager;

    private TurnData turnData;
    private Tile selectedTile;
    private List<Combatant> selectedTargets;
    private int maxRange;
    [Header("Events")]
    [SerializeField] private SignalSender onCameraZoomOut;

    public override void OnEnter()
    {
        base.OnEnter();

        turnData = battleManager.turnData;
        onCameraZoomOut.Raise();
        //display tiles
        gridManager.DisplayTilesInRange(turnData.combatant.tile, turnData.action.range, false, turnData.action.excludeStartingTile);
        
        PointerEventData pointer = new PointerEventData(EventSystem.current);
        pointer.position = Input.mousePosition;

        List<RaycastResult> raycastResults = new List<RaycastResult>();
        EventSystem.current.RaycastAll(pointer, raycastResults);

        if(raycastResults.Count > 0)
        {
            foreach(RaycastResult result in raycastResults)
            {  
                if(result.gameObject.GetComponent<Tile>() != null)
                {
                    Tile tile = result.gameObject.GetComponent<Tile>();
                    if(tile.targetability == Targetability.Targetable)
                    {
                        tile.Select();
                    }
                    break;
                }
            }
        }
        // GetNearestTargetableTile().Select();
    }

    public override void StateUpdate()
    {
        if(Input.GetButtonDown("Select"))
        {
            if(selectedTargets.Count > 0) 
            {
                battleManager.SetTargets(selectedTile, selectedTargets);
                if(turnData.action == turnData.combatant.rangedAttack && gridManager.GetMoveCost(turnData.combatant.tile, selectedTile) <= 1 && turnData.combatant.meleeAttack != null)
                {
                    turnData.action = turnData.combatant.meleeAttack;
                }
                stateMachine.ChangeState((int)BattleStateType.Execute); 
            }
            else 
            {
                Debug.Log("No target selected");
            }
        }
        if(Input.GetButtonDown("Cancel"))
        {
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
        selectedTile = null;
        gridManager.HideTiles();
    }

    public void OnSelectTile(GameObject tileObject)
    {     
        selectedTile = tileObject.GetComponent<Tile>();     
        selectedTargets = gridManager.GetTargetsInRange(selectedTile, turnData.action.aoe, turnData.action.targetFriendly, turnData.action.targetHostile);   
        if(turnData.action.knockback > 0)
        {
            Vector2 direction = (selectedTile.transform.position - turnData.combatant.tile.transform.position).normalized;
            List<Tile> path = new List<Tile>();
            if(selectedTile.occupier && selectedTile.occupier is EnemyCombatant)
                path = gridManager.GetRow(selectedTile, direction, turnData.action.knockback, true);
            gridManager.DisplayPath(path);
        } 
        if(turnData.action.lineAOE)
        {
            Vector2 direction = (selectedTile.transform.position - turnData.combatant.tile.transform.position).normalized;
            gridManager.DisplayLineAOE(selectedTile, direction, turnData.action.aoe, turnData.action.targetFriendly, turnData.action.targetHostile, turnData.action.stopAtOccupiedTile);
        }
        else
        {
            gridManager.DisplayAOE(selectedTile, turnData.action.aoe, turnData.action.targetFriendly, turnData.action.targetHostile);
        }  
    }

    // private Tile GetNearestTargetableTile()
    // {
    //     Tile closestTargetableTile = turnData.combatant.tile;
    //     int smallestDistance = turnData.action.range;
    //     foreach(Combatant target in gridManager.GetTargetsInRange(turnData.combatant.tile, turnData.action.range, turnData.action.targetFriendly, turnData.action.targetHostile))
    //     {
    //         int thisDistance = gridManager.GetMoveCost(target.tile, turnData.combatant.tile);
    //         if(thisDistance <= smallestDistance)
    //         {
    //             smallestDistance = thisDistance;
    //             closestTargetableTile = target.tile;
    //         }
    //     }
    //     return closestTargetableTile;
    // }
}
