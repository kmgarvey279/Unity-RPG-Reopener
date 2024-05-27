using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using StateMachineNamespace;
using System.Linq;

public enum BattleStateType
{
    BattleStart,
    TurnStart,
    PlayerTurn,
    TargetSelect,
    Execute,
    EnemyTurn,
    TurnEnd,
    BattleEnd,
    InterventionStart,
    ChangeTurn,
    Setup
}

public class BattleState : StateMachine.State
{
    [SerializeField] protected BattleStateType battleStateType;
    protected BattleManager battleManager;
    [SerializeField] protected GridManager gridManager;
    [SerializeField] protected BattleTimeline battleTimeline;

    protected List<SignalListenerBase> signalListeners = new List<SignalListenerBase>();

    public void Awake()
    {
        //state machine
        stateMachine = GetComponentInParent<StateMachine>(); 
        id = (int)battleStateType; 
        //battle manager
        battleManager = GetComponentInParent<BattleManager>();

        //signal listeners
        signalListeners = GetComponentsInChildren<SignalListenerBase>().ToList();
        foreach (MonoBehaviour script in signalListeners)
        {
            script.enabled = false;
        }
    }

    public override void OnEnter()
    {
        foreach (MonoBehaviour script in signalListeners)
        {
            if (script != null)
                script.enabled = true;
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
        foreach (MonoBehaviour script in signalListeners)
        {
            script.enabled = false;
        }
    }
}
