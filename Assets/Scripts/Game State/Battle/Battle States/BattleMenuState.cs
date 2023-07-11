using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Events;
using StateMachineNamespace;

[System.Serializable]
public class BattleMenuState : BattleState
{
    bool removeIntervention = false;
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
        commandMenuMain.HideAll();
        removeIntervention = false;
    }
}
