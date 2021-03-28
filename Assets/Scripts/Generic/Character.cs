using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using StateMachineNamespace;

public class Character : MonoBehaviour
{
    public CharacterInfo characterInfo;
    public MoveManager moveManager;
    public ActionManager actionManager;
    public Animator animator;
    public Rigidbody2D rigidbody;
    [Header("State")]
    public StateMachine stateMachine;
    public StateMachine.State moveState; 
    public StateMachine.State stunState;    
    public StateMachine.State dieState; 
    [Header("Direction & Target")]
    public Vector3 lookDirection;
    public Targeter targeter;
    [Header("Associated Prefabs")]
    public Afterimage afterimage; 
    
    public virtual void Start()
    {
        lookDirection = new Vector3(0,-1,0); 
        moveManager = GetComponentInChildren<MoveManager>();
        actionManager = GetComponentInChildren<ActionManager>();
        stateMachine = GetComponentInChildren<StateMachine>();
        targeter = GetComponent<Targeter>();
        animator = GetComponent<Animator>();
        rigidbody = GetComponent<Rigidbody2D>();
    }

    public virtual void ChangeLookDirection(Vector3 newDirection)
    {
        lookDirection = newDirection;
    }
}
