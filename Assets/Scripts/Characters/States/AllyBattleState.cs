using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using StateMachineNamespace;

public class AllyBattleState : StateMachine.State
{
    private PlayableCharacter character;

    public override void OnEnter()
    {
        character = GetComponentInParent<PlayableCharacter>();
        nextState = "";

        character.boxCollider.enabled = true;
        character.aiPath.canMove = true;
        character.setter.target = character.battlePosition;
    }

    public override void StateUpdate()
    {
    }

    public override void StateFixedUpdate()
    {
    }

    public override string CheckConditions()
    {
        return nextState;
    }

    public override void OnExit()
    {
    }	
}
