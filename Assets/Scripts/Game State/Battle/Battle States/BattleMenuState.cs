using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Events;
using StateMachineNamespace;

[System.Serializable]
public class BattleMenuState : BattleState
{
    [Header("Battle Menu")]
    [SerializeField] private CommandMenuMain commandMenuMain;
    [SerializeField] private BattleLog battleLog;
    [SerializeField] private SignalSenderGO onCameraFollow;

    public override void OnEnter()
    {
        base.OnEnter();

        //onCameraFollow.Raise(battleManager.TurnData.Combatant.gameObject);

        commandMenuMain.Display();
        PlayableCombatant playableCombatant = (PlayableCombatant)battleTimeline.CurrentTurn.Actor;
        playableCombatant.BattlePartyPanel.Highlight(true);
        battleLog.ToggleDisplay(true);
    }

    public override void StateUpdate()
    {
        if (Input.GetButtonDown("Intervention"))
        {
            battleTimeline.CurrentTurn.PauseTurn();
            battleTimeline.AddTurn(TurnType.Intervention, battleTimeline.CurrentTurn.Actor);
            stateMachine.ChangeState((int)BattleStateType.ChangeTurn);
        }
    }

    public override void StateFixedUpdate()
    {
    }

    public override void OnExit()
    {
        base.OnExit();
        commandMenuMain.HideAll();
    }
}
