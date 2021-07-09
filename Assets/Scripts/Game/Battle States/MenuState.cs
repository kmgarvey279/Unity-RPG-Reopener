using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Events;
using StateMachineNamespace;

[System.Serializable]
public class MenuState : BattleState
{
    [Header("Battle Menu")]
    [SerializeField] private CommandMenu commandMenu;
    private BattleManager battleManager;
    private TurnData turnData;
    [Header("Events")]
    public SignalSenderGO onCameraZoomIn;

    public override void Start()
    {
        base.Start();
        battleManager = GetComponentInParent<BattleManager>();
    }
    
    public override void OnEnter()
    {
        turnData = battleManager.turnData;

        onCameraZoomIn.Raise(turnData.combatant.gameObject);

        commandMenu.DisplayMenu();
    }

    public override void StateUpdate()
    {

    }

    public override void StateFixedUpdate()
    {

    }

    public override void OnExit()
    {
        commandMenu.HideMenus();
    }
}
