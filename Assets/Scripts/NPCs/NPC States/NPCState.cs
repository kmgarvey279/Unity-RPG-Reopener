using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using StateMachineNamespace;

public class NPCState : StateMachine.State
{
    [HideInInspector]
    public Animator myAnimator;
    [HideInInspector]
    public Rigidbody2D myRB;
    // [HideInInspector]
    public ActionManager actionManager;
    [HideInInspector]
    public NPCMoveManager moveManager;

    // Start is called before the first frame update
    public void Awake()
    {
        myRB = GetComponent<Rigidbody2D>();
        myAnimator = GetComponent<Animator>(); 
        actionManager = gameObject.transform.parent.gameObject.GetComponentInChildren<ActionManager>();
        moveManager = GetComponentInParent<NPCMoveManager>();
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
