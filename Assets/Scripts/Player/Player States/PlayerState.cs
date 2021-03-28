using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using StateMachineNamespace;

[System.Serializable]
public class PlayerState : StateMachine.State
{
    [HideInInspector]
    public Character character;

    private void Start()
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
