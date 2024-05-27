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
        battleTimeline.CurrentTurn.Actor.EndTimeStop();

        //if (battleTimeline.CurrentTurn.Actor is PlayableCombatant)
        //{
        //    PlayableCombatant playableCombatant = (PlayableCombatant)battleTimeline.CurrentTurn.Actor;
        //    if (playableCombatant != null) 
        //    {
        //        playableCombatant.ModifyGuard(1);
        //    }
        //}
        stateMachine.ChangeState((int)BattleStateType.PlayerTurn);
    }
}