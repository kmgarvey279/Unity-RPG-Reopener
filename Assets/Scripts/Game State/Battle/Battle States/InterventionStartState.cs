using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.EventSystems;

[System.Serializable]
public class InterventionStartState : BattleState
{
    private int actorIndex = 0;
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
            Combatant actor = battleManager.GetCombatants(CombatantType.Player)[actorIndex];
            battleTimeline.UpdateTurnActor(battleTimeline.CurrentTurn, actor);
            stateMachine.ChangeState((int)BattleStateType.Menu);
        }
        //change actors
        else if (Input.GetButtonDown("Switch"))
        {
            List<Combatant> combatants = battleManager.GetCombatants(CombatantType.Player);
            if(combatants.Count > 1) 
            { 
                SwapActor();
            }
        }
        //cancel
        if (Input.GetButtonDown("Cancel"))
        {
            onInterventionEnd.Raise();

            battleTimeline.RemoveTurn(battleTimeline.CurrentTurn, false);
            stateMachine.ChangeState((int)BattleStateType.ChangeTurn);
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

        List<Combatant> combatants = battleManager.GetCombatants(CombatantType.Player);

        if (battleTimeline.CurrentTurn.Actor && !battleTimeline.CurrentTurn.IsDead)
        {
            actorIndex = combatants.IndexOf(battleTimeline.CurrentTurn.Actor);
        }
        else
        {
            actorIndex = 0;
        }
        //activate current actor
        PlayableCombatant firstActor = (PlayableCombatant)combatants[actorIndex];
        firstActor.EndTimeStop();
        firstActor.BattlePartyPanel.Highlight(true);
    }

    private void SwapActor()
    {
        List<Combatant> combatants = battleManager.GetCombatants(CombatantType.Player);

        //deactivate current actor
        PlayableCombatant originalActor = (PlayableCombatant)combatants[actorIndex];
        originalActor.StartTimeStop();
        originalActor.BattlePartyPanel.Highlight(false);

        //activate current actor
        int newIndex = actorIndex + 1;
        if (newIndex >= combatants.Count)
        {
            newIndex = 0;
        }
        actorIndex = newIndex;
        PlayableCombatant newActor = (PlayableCombatant)combatants[newIndex];
        newActor.EndTimeStop();
        newActor.BattlePartyPanel.Highlight(true);
    }
}