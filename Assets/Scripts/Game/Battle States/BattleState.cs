using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using StateMachineNamespace;

public enum BattleStateType
{
    Start,
    Menu,
    Move,
    TargetSelect,
    TileSelect,
    Execute,
    EnemyTurn,
    End
}

public class BattleState : StateMachine.State
{
    public BattleStateType battleStateType;

    public virtual void Start()
    {
        id = (int)battleStateType;
        stateMachine = GetComponentInParent<StateMachine>();
    }

    public override void OnEnter()
    {
    }

    public override void StateUpdate()
    {
    }

    public override void StateFixedUpdate()
    {
    }

    public override void OnExit()
    {
    }
}
