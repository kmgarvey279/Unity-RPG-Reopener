using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using StateMachineNamespace;

[System.Serializable]
public class PausedState : OverworldState
{
    //[SerializeField] private PauseMenuManager pauseMenuManager;
    [SerializeField] private SignalSenderInt onChangePauseMenuState;

    public override void OnEnter()
    {
        base.OnEnter();

        overworldManager.PauseStart();
        onChangePauseMenuState.Raise((int)CommandMenuStateType.Main);
    }

    public override void StateUpdate()
    {
    }

    public override void StateFixedUpdate()
    {

    }

    public override void OnExit()
    {
        base.OnExit();

        overworldManager.PauseEnd();
    }

    public void OnExitPauseMenu()
    {
        stateMachine.ChangeState((int)OverworldStateType.FreeMove);
    }
}
