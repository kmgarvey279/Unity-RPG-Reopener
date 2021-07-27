using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Events;
using StateMachineNamespace;
using UnityEngine.EventSystems;

[System.Serializable]
public class PlayerMoveState : BattleState
{
    [SerializeField] private GridManager gridManager;

    private TurnData turnData;
    private Tile selectedTile;

    [Header("Unity Events (Signals)")]
    [SerializeField] private SignalSender onCameraZoomOut;
    [SerializeField] private SignalSenderGO onCameraZoomIn;

    public override void OnEnter()
    {
        base.OnEnter();

        turnData = battleManager.turnData;
        onCameraZoomOut.Raise();

        int range = turnData.combatant.GetStatValue(StatType.MoveRange);
        gridManager.DisplayTilesInRange(turnData.combatant.tile, range, -1, true);

    }

    public override void StateUpdate()
    {
        if(Input.GetButtonDown("Select"))
        {
            OnConfirmTile();
        }
        if(Input.GetButtonDown("Cancel"))
        {
            selectedTile = null;
            gridManager.HideTiles();

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
    }

    public void OnSelectTile(GameObject tileObject)
    {     
        selectedTile = tileObject.GetComponent<Tile>();
    }

    public void OnConfirmTile()
    {                
        gridManager.HideTiles();
        turnData.combatant.Move(selectedTile);
        selectedTile = null; 
    }

    public void OnMoveComplete()
    {
        if(turnData.action.fixedTarget)
        {
            battleManager.stateMachine.ChangeState((int)BattleStateType.TargetSelect);
        }
        else
        {
            battleManager.stateMachine.ChangeState((int)BattleStateType.TileSelect);
        }
    }


}
