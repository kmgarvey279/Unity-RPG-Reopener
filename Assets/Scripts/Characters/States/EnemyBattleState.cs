using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using StateMachineNamespace;

public class EnemyBattleState : StateMachine.State
{
    private Character character;

    public override void OnEnter()
    {
        character = GetComponentInParent<Character>();
        nextState = "";
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

