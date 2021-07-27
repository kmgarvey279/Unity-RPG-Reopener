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
        int aoe = turnData.action.aoe;
        gridManager.DisplayTilesInRange(turnData.combatant.tile, range, aoe, false);
    }

    public override void StateUpdate()
    {
        if(Input.GetButtonDown("Select"))
        {
            OnConfirmTile();
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
        gridManager.DisplayAOE(selectedTile, turnData.action.aoe);
    }

    public void OnConfirmTile()
    {                 
        turnData.targets.AddRange(gridManager.GetTargetsInAOE());
        turnData.targetedTile = selectedTile;
        stateMachine.ChangeState((int)BattleStateType.Execute);
    }
}
