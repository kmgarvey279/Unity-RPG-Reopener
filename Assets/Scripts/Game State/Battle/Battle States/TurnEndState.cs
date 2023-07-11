using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using StateMachineNamespace;

[System.Serializable]
public class TurnEndState : BattleState
{
    [SerializeField] private BattleEventQueue battleEventQueue;
    [SerializeField] private SignalSender onInterventionEnd;
    private WaitForSeconds waitZeroPointTwoFive = new WaitForSeconds(0.25f);
    public override void OnEnter()
    {
        Debug.Log("Entering Turn End State");
        base.OnEnter();
        StartCoroutine(EndTurnCo());
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
    
    public IEnumerator EndTurnCo()
    {
        yield return waitZeroPointTwoFive;

        //trigger turn end effects
        if (battleTimeline.CurrentTurn.TurnType != TurnType.Intervention && !battleTimeline.CurrentTurn.Actor.IsKOed)
        {
            //add turn start effects to queue
            yield return StartCoroutine(battleTimeline.CurrentTurn.Actor.OnTurnEndCo());

            //execute any turn end effects
            yield return StartCoroutine(battleEventQueue.ExhaustQueueCo());
            yield return waitZeroPointTwoFive;

            //check combatants for ko
            List<Combatant> allCombatants = battleManager.GetCombatants(CombatantType.All);
            for (int i = allCombatants.Count - 1; i >= 0; i--)
            {
                //resolve health change
                allCombatants[i].ResolveHealthChange();
                if (allCombatants[i].IsKOed)
                {
                    battleManager.KOCombatant(allCombatants[i]);
                }
            }
            yield return waitZeroPointTwoFive;
        }

        if (!battleTimeline.CurrentTurn.Actor.IsKOed)
        {
            //unhighlight party panel
            if (battleTimeline.CurrentTurn.Actor is PlayableCombatant)
            {
                PlayableCombatant playableCombatant = (PlayableCombatant)battleTimeline.CurrentTurn.Actor;
                playableCombatant.BattlePartyPanel.Highlight(false);
            }

            //destroy previous turn panel
            battleTimeline.RemoveTurn(battleTimeline.CurrentTurn, false);
        }

        //switch to change turn state
        stateMachine.ChangeState((int)BattleStateType.ChangeTurn);
    }
}
