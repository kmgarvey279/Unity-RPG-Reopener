using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using StateMachineNamespace;

[System.Serializable]
public class PlayerDieState : PlayerState
{

    public override void OnEnter()
    {
        character.animator.SetTrigger("Die");
    }

    public override void OnExit()
    {
    }
}
