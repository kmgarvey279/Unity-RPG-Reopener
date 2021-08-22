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

    [Header("Events")]
    [SerializeField] private SignalSender onCameraZoomOut;

    public override void OnEnter()
    {
        base.OnEnter();

        turnData = battleManager.turnData;
        onCameraZoomOut.Raise();
        //display tiles
        int range = turnData.action.range;
        gridManager.DisplayTilesInRange(turnData.combatant.tile, range, false);
    }

    public override void StateUpdate()
    {
        if(Input.GetButtonDown("Select"))
        {
             if(turnData.targets.Count > 0) 
            {
                turnData.targetedTile = selectedTile;
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
        gridManager.DisplayAOE(selectedTile, turnData.action.aoe, turnData.action.targetFriendly, turnData.action.targetHostile);
    }

    public void OnConfirmTile()
    {                
        // if(turnData.targets.Count > 0) 
        // {
        //     turnData.targetedTile = selectedTile;
        //     stateMachine.ChangeState((int)BattleStateType.Execute); 
        // }
        // else 
        // {
        //     Debug.Log("No target selected");
        // }

    }
}
