using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using StateMachineNamespace;

[System.Serializable]
public class NPCStunState : NPCState
{

    public override void OnEnter()
    {
        character.animator.SetBool("Stun", true);
    }

    public override void OnExit()
    {
        character.animator.SetBool("Stun", false);
    }
}
