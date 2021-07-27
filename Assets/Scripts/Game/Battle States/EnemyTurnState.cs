using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Events;
using StateMachineNamespace;

[System.Serializable]
public class EnemyTurnState : BattleState
{
    private TurnData turnData;

    [Header("Unity Events")]
    public SignalSender onCameraZoomOut;

    public override void OnEnter()
    {
        base.OnEnter();
        
        turnData = battleManager.turnData;

        onCameraZoomOut.Raise();
        Move();
    }

    public override void StateUpdate()
    {
        
    }

    public override void StateFixedUpdate()
    {

    }

    public override void OnExit()
    {

    }

    private void Move()
    {
        // int moveRange = turnData.combatant.GetStatValue(StatType.MoveRange);
        // Tile nearestTile = gridManager.GetClosestTileInRange(turnData.combatant.tile, target.tile, moveRange);
        // turnData.combatant.Move(nearestTile);
    }
}

