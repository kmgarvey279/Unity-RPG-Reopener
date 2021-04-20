using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using StateMachineNamespace;

[System.Serializable]
public class PlayerState : StateMachine.State
{
    [HideInInspector]
    public Character character;
    public Rigidbody2D rigidbody;
    public Animator animator;

    private void Start()
    {
        character = GetComponentInParent<Character>();
        rigidbody = GetComponentInParent<Rigidbody2D>();
        animator = GetComponentInParent<Animator>();
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

    public override string CheckConditions()
    {
        return null;
    }

    public override void OnExit()
    {

    }
}
