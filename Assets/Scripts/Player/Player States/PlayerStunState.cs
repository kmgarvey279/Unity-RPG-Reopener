using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using StateMachineNamespace;

[System.Serializable]
public class PlayerStunState : PlayerState
{

    public override void OnEnter()
    {
        animator.SetBool("Stun", true);
    }

    public override void OnExit()
    {
        animator.SetBool("Stun", false);
    }
}
