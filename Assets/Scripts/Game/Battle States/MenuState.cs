using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using StateMachineNamespace;

[System.Serializable]
public class MenuState : StateMachine.State
{
    [Header("Battle Menu")]
    [SerializeField] private CommandMenu commandMenu;
    [SerializeField] private BattleManager battleManager;
    public CameraManager cameraManager;

    private void Start()
    {
        battleManager = GetComponentInParent<BattleManager>();
    }
    
    public override void OnEnter()
    {
        cameraManager.SetTarget(battleManager.turnData.combatant.transform);

        commandMenu.DisplayMenu();
    }

    public override void StateUpdate()
    {

    }

    public override void StateFixedUpdate()
    {

    }

    public override string CheckConditions()
    {
        return nextState;
    }

    public override void OnExit()
    {
        commandMenu.HideMenus();
    }
}
