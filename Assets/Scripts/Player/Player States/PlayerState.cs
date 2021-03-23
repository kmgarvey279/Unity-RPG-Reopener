using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using StateMachineNamespace;

[System.Serializable]
public class PlayerState : StateMachine.State
{
    [HideInInspector]
    public Rigidbody2D playerRB;
    [HideInInspector]
    public Animator animator;
    [HideInInspector]
    public PlayerMoveManager playerMoveManager;

    private void Start()
    {
        playerRB = GetComponentInParent<Rigidbody2D>();
        animator = GetComponentInParent<Animator>();
        playerMoveManager = GetComponentInParent<PlayerMoveManager>();
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
