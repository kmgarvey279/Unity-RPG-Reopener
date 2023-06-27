using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using StateMachineNamespace;

public enum BattleStateType
{
    BattleStart,
    TurnStart,
    Menu,
    TargetSelect,
    Execute,
    EnemyTurn,
    TurnEnd,
    BattleEnd,
    InterventionStart,
    ChangeTurn
}

public class BattleState : StateMachine.State
{
    [SerializeField] protected BattleStateType battleStateType;
    protected BattleManager battleManager;
    [SerializeField] protected GridManager gridManager;
    [SerializeField] protected BattleTimeline battleTimeline;
    [Header("Unity Events (Listeners)")]
    [SerializeField] protected List<MonoBehaviour> signalListeners = new List<MonoBehaviour>();

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
