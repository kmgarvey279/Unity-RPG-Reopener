using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using StateMachineNamespace;

[System.Serializable]
public class MenuState : OverworldState
{
    [SerializeField] private MainPauseMenu menu; 

    public override void OnEnter()
    {
        base.OnEnter();
        menu.Display();
    }

    public override void StateUpdate()
    {
        if(Input.GetButtonDown("Menu"))
        {
            stateMachine.ChangeState((int)OverworldStateType.FreeMove);
        }
    }

    public override void StateFixedUpdate()
    {

    }

    public override void OnExit()
    {
        base.OnExit();
        menu.Hide();
    }
}
