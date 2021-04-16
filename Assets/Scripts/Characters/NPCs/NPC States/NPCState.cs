using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using StateMachineNamespace;

public class NPCState : StateMachine.State
{
    public Character character;
    public Rigidbody2D rigidbody;
    public Animator animator;

    // Start is called before the first frame update
    public void Awake()
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
