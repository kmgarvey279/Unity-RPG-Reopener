using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Events;
using StateMachineNamespace;

[System.Serializable]
public class EnemyTurnState : BattleState
{
    // private EnemyCombatant enemyCombatant;
    // [SerializeField] GridManager gridManager;

    public override void OnEnter()
    {
        base.OnEnter();
        // enemyCombatant = (EnemyCombatant)turnData.combatant;

        // onCameraZoomOut.Raise();
        // DecisionPhase();
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

    // private void DecisionPhase()
    // {
    //     PotentialAction potentialAction = enemyCombatant.GetTurnAction();
    //     battleManager.SetAction(potentialAction.action);
    //     battleManager.SetMoveCost(gridManager.GetMoveCost(enemyCombatant.tile, potentialAction.destinationTile));
    //     battleManager.SetTargets(potentialAction.targetedTile, potentialAction.targets);

    //     StartCoroutine(MovePhase(potentialAction.destinationTile));
    // }

    // private IEnumerator MovePhase(Tile destinationTile)
    // {
    //     Debug.Log("Enemy is on: " + enemyCombatant.tile.x +"/"+enemyCombatant.tile.y);
    //     Debug.Log("Enemy will move to tile: " + destinationTile.x +"/"+destinationTile.y);
    //     yield return new WaitForSeconds(0.2f);
    //     if(enemyCombatant.tile != destinationTile)
    //     {
    //         Debug.Log("Enemy will move");
    //         List<Tile> path = gridManager.GetPath(enemyCombatant.tile, destinationTile);
    //         gridManager.DisplayPath(path);
    //         enemyCombatant.gridMovement.Move(path, MovementType.Move); 
    //     }
    //     else
    //     {
    //         Debug.Log("Enemy will not move");
    //         StartCoroutine(ActionPhase());
    //     }
    // }

    // public void OnMoveComplete()
    // {
    //     Debug.Log("Enemy move complete");
    //     gridManager.HideTiles();
    //     StartCoroutine(ActionPhase());
    // }

    // private IEnumerator ActionPhase()
    // {
    //     gridManager.DisplayAOE(turnData.combatant.tile, turnData.action.range, turnData.action.targetHostile, turnData.action.targetFriendly);
    //     yield return new WaitForSeconds(0.2f);
    //     Debug.Log("Enemy action start");
    //     stateMachine.ChangeState((int)BattleStateType.Execute);
    // }
}

