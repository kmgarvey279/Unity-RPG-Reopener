using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.EventSystems;

[System.Serializable]
public class InterventionStartState : BattleState
{
    [SerializeField] private TargetInfo targetInfo;
    [SerializeField] private SignalSender onInterventionStart;
    [SerializeField] private SignalSender onInterventionEnd;
    private WaitForSeconds waitZeroPointFive = new WaitForSeconds(0.5f);
    public override void OnEnter()
    {
        base.OnEnter();
        onInterventionStart.Raise();
        StartCoroutine(ResolveInterventionStartCo());
    }

    public override void StateUpdate()
    {
        //confirm
        if (Input.GetButtonDown("Submit"))
        {
            stateMachine.ChangeState((int)BattleStateType.Menu);
        }
        //cancel
        else if (Input.GetButtonDown("Cancel"))
        {
            onInterventionEnd.Raise();

            battleTimeline.RemoveTurn(battleTimeline.CurrentTurn, false);
            stateMachine.ChangeState((int)BattleStateType.ChangeTurn);
        }
        else if (Input.GetButtonDown("QueueIntervention1"))
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

    private IEnumerator ResolveInterventionStartCo()
    {
        yield return waitZeroPointFive;
        PlayableCombatant actor = (PlayableCombatant)battleTimeline.CurrentTurn.Actor;

        actor.EndTimeStop();
        actor.BattlePartyPanel.Highlight(true);
    }
}