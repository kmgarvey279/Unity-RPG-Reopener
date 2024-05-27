using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Events;
using StateMachineNamespace;

[System.Serializable]
public class PlayerTurnState : BattleState
{
    [Header("Battle Menu")]
    [SerializeField] private SignalSenderGO onCameraFollow;
    [SerializeField] private CommandMenuManager commandMenuManager;

    public override void OnEnter()
    {
        base.OnEnter();

        //onCameraFollow.Raise(battleTimeline.CurrentTurn.Actor.gameObject);

        //highlight actor
        PlayableCombatant playableCombatant = (PlayableCombatant)battleTimeline.CurrentTurn.Actor;
        playableCombatant.BattlePartyPanel.OnTurnStart();
        playableCombatant.ToggleHighlight(true);

        commandMenuManager.Display();
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

        //unhighlight actor
        battleTimeline.CurrentTurn.Actor.ToggleHighlight(false);

        commandMenuManager.Hide();
    }
}
