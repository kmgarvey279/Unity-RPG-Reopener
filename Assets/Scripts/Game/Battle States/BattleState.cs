using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using StateMachineNamespace;

public enum BattleStateType
{
    Start,
    Menu,
    Move,
    TileSelect,
    Execute,
    EnemyTurn,
    End
}

public class BattleState : StateMachine.State
{
    public BattleStateType battleStateType;
    [HideInInspector] public BattleManager battleManager;
    [HideInInspector] public TurnData turnData;
    [Header("Unity Events (Listeners)")]
    public List<MonoBehaviour> signalListeners;

    public void Awake()
    {
        //state machine
        stateMachine = GetComponentInParent<StateMachine>(); 
        id = (int)battleStateType; 
        //battle manager
        battleManager = GetComponentInParent<BattleManager>();  
    }

    public override void OnEnter()
    {
        if(signalListeners.Count > 0)
        {
            foreach (MonoBehaviour script in signalListeners)
            {
                script.enabled = true;
            }
        }
        turnData = battleManager.turnData;  
    }

    public override void StateUpdate()
    {
    }

    public override void StateFixedUpdate()
    {
    }

    public override void OnExit()
    {
        if(signalListeners.Count > 0)
        {
            foreach (MonoBehaviour script in signalListeners)
            {
                script.enabled = false;
            }
        }
    }
}
