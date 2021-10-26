using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Events;
using StateMachineNamespace;

[System.Serializable]
public class EnemyTurnState : BattleState
{
    private TurnData turnData;
    private EnemyCombatant enemyCombatant;
    [SerializeField] GridManager gridManager;

    [Header("Unity Events")]
    public SignalSender onCameraZoomOut;

    public override void OnEnter()
    {
        base.OnEnter();
        
        turnData = battleManager.turnData;
        enemyCombatant = (EnemyCombatant)turnData.combatant;

        onCameraZoomOut.Raise();
        DecisionPhase();
    }

    public override void StateUpdate()
    {
        
    }

    public override void StateFixedUpdate()
    {

    }

    public override void OnExit()
    {
        base.OnExit();
    }

    private void DecisionPhase()
    {
        EnemyActionData actionData = enemyCombatant.GetActionData();

        battleManager.SetAction(actionData.action);
        battleManager.SetMoveCost(gridManager.GetMoveCost(enemyCombatant.tile, actionData.destinationTile));
        battleManager.SetTargets(actionData.targetedTile, actionData.targets);

        StartCoroutine(MovePhase(actionData.destinationTile));
    }

    private IEnumerator MovePhase(Tile destinationTile)
    {
        yield return new WaitForSeconds(0.2f);
        
        if(enemyCombatant.tile != destinationTile)
        {
            List<Tile> path = gridManager.GetPath(enemyCombatant.tile, destinationTile);
            enemyCombatant.gridMovement.Move(path, MovementType.Move); 
        }
        else
        {
            StartCoroutine(ActionPhase());
        }
    }

    public void OnMoveComplete()
    {
        StartCoroutine(ActionPhase());
    }

    private IEnumerator ActionPhase()
    {
        yield return new WaitForSeconds(0.2f);
        stateMachine.ChangeState((int)BattleStateType.Execute);
    }
}

