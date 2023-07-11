using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using StateMachineNamespace;

[System.Serializable]
public class TurnStartState : BattleState
{
    [SerializeField] private BattlePartyHUD battlePartyHUD;
    [SerializeField] private BattleEventQueue battleEventQueue;
    //cache wait for seconds
    private WaitForSeconds waitZeroPointTwoFive = new WaitForSeconds(0.25f);
    private WaitForSeconds waitZeroPointFive = new WaitForSeconds(0.5f);

    public override void OnEnter()
    {
        Debug.Log("Entering Turn Start State");
        base.OnEnter();
        StartCoroutine(StartTurnCo());
    }

    public override void StateUpdate()
    {
        if (Input.GetButtonDown("QueueIntervention1"))
        {
            if (battleManager.InterventionCheck(0))
            {
                if (Input.GetButton("Shift"))
                {
                    battleTimeline.RemoveLastIntervention(battleManager.PlayableCombatants[0]);
                }
                else
                {
                    battleTimeline.AddInterventionToQueue(battleManager.PlayableCombatants[0]);
                }
            }
        }
        else if (Input.GetButtonDown("QueueIntervention2"))
        {
            if (battleManager.InterventionCheck(1))
            {
                if (Input.GetButton("Shift"))
                {
                    battleTimeline.RemoveLastIntervention(battleManager.PlayableCombatants[1]);
                }
                else
                {
                    battleTimeline.AddInterventionToQueue(battleManager.PlayableCombatants[1]);
                }
            }
        }
        else if (Input.GetButtonDown("QueueIntervention3"))
        {
            if (battleManager.InterventionCheck(2))
            {
                if (Input.GetButton("Shift"))
                {
                    battleTimeline.RemoveLastIntervention(battleManager.PlayableCombatants[2]);
                }
                else
                {
                    battleTimeline.AddInterventionToQueue(battleManager.PlayableCombatants[2]);
                }
            }
        }
    }


    public override void StateFixedUpdate()
    {

    }

    public override void OnExit()
    {
        base.OnExit();
    }

    private IEnumerator StartTurnCo()
    {
        yield return waitZeroPointTwoFive;
        //update chain value
        //battleManager.ChainCheck();
        //add turn start effects to queue
        yield return StartCoroutine(battleTimeline.CurrentTurn.Actor.OnTurnStartCo());
        //execute any turn start effects
        yield return StartCoroutine(battleEventQueue.ExhaustQueueCo());
        yield return waitZeroPointTwoFive;

        if (battleTimeline.CurrentTurn.Actor is PlayableCombatant)
        {
            stateMachine.ChangeState((int)BattleStateType.Menu);
        }
        else if (battleTimeline.CurrentTurn.Actor is EnemyCombatant)
        {
            stateMachine.ChangeState((int)BattleStateType.EnemyTurn);
        }
    }
}
