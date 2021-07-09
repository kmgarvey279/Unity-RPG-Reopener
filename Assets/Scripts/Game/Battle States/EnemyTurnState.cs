using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Events;
using StateMachineNamespace;

[System.Serializable]
public class EnemyTurnState : BattleState
{
    private BattleManager battleManager;
    private Combatant combatant;
    [Header("Unity Events")]
    public SignalSender onCameraZoomOut;

    public override void Start()
    {
        base.Start();
        battleManager = GetComponentInParent<BattleManager>();
        // combatant = battleManager.currentTurnSlot.combatant;
    }

    public override void OnEnter()
    {
        onCameraZoomOut.Raise();
    }

    public override void StateUpdate()
    {
        
    }

    public override void StateFixedUpdate()
    {

    }

    public override void OnExit()
    {

    }
}

