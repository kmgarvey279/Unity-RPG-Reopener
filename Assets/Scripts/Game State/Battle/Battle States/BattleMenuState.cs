using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Events;
using StateMachineNamespace;

[System.Serializable]
public class BattleMenuState : BattleState
{
    [Header("Battle Menu")]
    [SerializeField] private CommandMenu commandMenu;

    [Header("Events")]
    public SignalSenderGO onCameraZoomIn;
    
    public override void OnEnter()
    {
        base.OnEnter();
        // onCameraZoomIn.Raise(turnData.combatant.gameObject);
        commandMenu.DisplayMenu();
    }

    public override void StateUpdate()
    {
        if(Input.GetButtonDown("Cancel"))
        {
            commandMenu.ExitCurrentMenu();
        }
    }

    public override void StateFixedUpdate()
    {
    }

    public override void OnExit()
    {
        base.OnExit();
        commandMenu.HideMenus();
    }
}
