using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using StateMachineNamespace;

[System.Serializable]
public class NPCDieState : NPCState
{
    public SignalSenderGO targetRemove;

    public override void OnEnter()
    {
        animator.SetBool("Die", true);
        targetRemove.Raise(this.gameObject);
    }

    public override void OnExit()
    {

    }
}
