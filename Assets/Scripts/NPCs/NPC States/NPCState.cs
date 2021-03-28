using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using StateMachineNamespace;

public class NPCState : StateMachine.State
{
    public Character character;

    // Start is called before the first frame update
    public void Awake()
    {
        character = transform.root.gameObject.GetComponent<Character>();
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
