using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using StateMachineNamespace;

[System.Serializable]
public class DialogueState : OverworldState
{
    [SerializeField] private TextBox textBox;

    public override void OnEnter()
    {
        base.OnEnter();
        overworldData.lockInput = true;
    }

    public override void StateUpdate()
    {
        if(Input.GetButtonDown("Select"))
        {
            textBox.AdvanceText();
        }
    }

    public override void StateFixedUpdate()
    {

    }

    public override void OnExit()
    {
        base.OnExit();
        overworldData.lockInput = false;
        StartCoroutine(DialogueTriggerCooldown());
    }

    public void OnUnlockInput()
    {
        stateMachine.ChangeState((int)OverworldStateType.FreeMove);
    }

    private IEnumerator DialogueTriggerCooldown()
    {
        overworldData.interactTriggerCooldown = true;
        yield return new WaitForSeconds(0.5f);
        overworldData.interactTriggerCooldown = false;
    }
}
