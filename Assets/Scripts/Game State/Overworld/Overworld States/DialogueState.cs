using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using StateMachineNamespace;

[System.Serializable]
public class DialogueState : OverworldState
{
    [SerializeField] private DialogueBox dialogueBox;
    [SerializeField] private SignalSender onPauseStart;
    [SerializeField] private SignalSender onPauseEnd;

    public override void OnEnter()
    {
        base.OnEnter();
        overworldManager.PauseStart();
    }

    public override void StateUpdate()
    {
        if (Input.GetButtonDown("Select"))
        {
            dialogueBox.AdvanceText();
        }
    }

    public override void StateFixedUpdate()
    {

    }

    public override void OnExit()
    {
        base.OnExit();
        overworldManager.PauseEnd();
    }
}
