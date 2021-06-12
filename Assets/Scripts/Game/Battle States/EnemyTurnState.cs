using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using StateMachineNamespace;

[System.Serializable]
public class EnemyTurnState : StateMachine.State
{
    private BattleManager battleManager;
    private Combatant combatant;
    public CameraManager cameraManager;

    private void Start()
    {
        battleManager = GetComponentInParent<BattleManager>();
        // combatant = battleManager.currentTurnSlot.combatant;
    }

    public override void OnEnter()
    {
        cameraManager.SetTarget(combatant.gameObject.transform);
        // enemyTargetA
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

    }
}

