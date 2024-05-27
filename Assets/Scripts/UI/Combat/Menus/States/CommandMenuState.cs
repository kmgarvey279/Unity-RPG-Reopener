using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using StateMachineNamespace;
using System.Linq;

public enum CommandMenuStateType
{
    Main, 
    Skills,
    Items,
    Tactics,
    Party,
    Escape,
    Inactive
}

public class CommandMenuState : StateMachine.State
{
    [SerializeField] protected CommandMenuStateType commandMenuStateType;
    [SerializeField] protected GameObject lastButton;
    [Header("External References")]
    [SerializeField] protected BattleManager battleManager;
    [SerializeField] protected BattleTimeline battleTimeline;
    [Header("Events (Senders)")]
    [SerializeField] protected SignalSenderInt onChangeBattleState;

    protected List<SignalListenerBase> signalListeners = new List<SignalListenerBase>();

    public virtual void Awake()
    {
        //state machine
        stateMachine = GetComponentInParent<StateMachine>();
        id = (int)commandMenuStateType;

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

    public virtual void Reset()
    {
        lastButton = null;
    }
}
