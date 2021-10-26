using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Events;
using StateMachineNamespace;

[System.Serializable]
public class MenuState : BattleState
{
    private TurnData turnData;

    [Header("Battle Menu")]
    [SerializeField] private CommandMenu commandMenu;

    [Header("Events")]
    public SignalSenderGO onCameraZoomIn;
    
    public override void OnEnter()
    {
        base.OnEnter();
        turnData = battleManager.turnData;  
        
        //checks if player is canceling a move
        if(turnData.combatant.transform.position != turnData.startingPosition)
        {
            battleManager.SetMoveCost(0);
            ResetPosition();
        }

        // onCameraZoomIn.Raise(turnData.combatant.gameObject);

        commandMenu.DisplayMenu();
    }

    private void ResetPosition()
    {
        turnData.combatant.transform.position = turnData.startingPosition;
        turnData.combatant.SetLookDirection(new Vector2(turnData.startingDirection.x, turnData.startingDirection.y));
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
        commandMenu.HideMenus();
    }
}
