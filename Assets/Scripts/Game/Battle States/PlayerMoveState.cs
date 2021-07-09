using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Events;
using StateMachineNamespace;
using UnityEngine.EventSystems;

[System.Serializable]
public class PlayerMoveState : BattleState
{
    private BattleManager battleManager;
    public GridManager gridManager;

    private TurnData turnData;

    [Header("Unity Events")]
    public SignalSender onCameraZoomOut;
    public SignalSenderGO onCameraZoomIn;

    public override void Start()
    {
        base.Start();
        battleManager = GetComponentInParent<BattleManager>();
    }

    public override void OnEnter()
    {
        turnData = battleManager.turnData;

        onCameraZoomOut.Raise();

        int moveRange = turnData.combatant.GetStatValue(StatType.MoveRange);
        gridManager.DisplayTilesInRange(turnData.combatant.tile, moveRange, -1);
    }

    public override void StateUpdate()
    {
    }

    public override void StateFixedUpdate()
    {

    }

    public override void OnExit()
    {
        gridManager.HideTiles();
    }

    public void OnSelectTile(GameObject tileObject)
    {     
        onCameraZoomIn.Raise(turnData.combatant.gameObject);
        turnData.combatant.Move(tileObject.GetComponent<Tile>());
    }

    public void OnMoveComplete()
    {
        turnData.hasMoved = true;
        if(turnData.action == null)
        {
            stateMachine.ChangeState((int)BattleStateType.Menu);
        }
        else
        {
            if(turnData.action.fixedTarget)
            {
                stateMachine.ChangeState((int)BattleStateType.TargetSelect);
            }
            else
            {
                stateMachine.ChangeState((int)BattleStateType.TileSelect);
            }
        }
    }


}
