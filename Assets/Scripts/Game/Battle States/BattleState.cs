using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using StateMachineNamespace;

public enum BattleStateType
{
    Start,
    Menu,
    Move,
    TargetSelect,
    TileSelect,
    Execute,
    EnemyTurn,
    End
}

public class BattleState : StateMachine.State
{
    public BattleStateType battleStateType;
    [HideInInspector] public BattleManager battleManager;
    [Header("Unity Events (Listeners)")]
    public List<MonoBehaviour> signalListeners;

    public virtual void Start()
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
