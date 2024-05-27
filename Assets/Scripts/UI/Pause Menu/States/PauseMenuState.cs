using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using StateMachineNamespace;
using System.Linq;

public enum PauseMenuStateType
{
    Inactive,
    Main,
    Party,
    Equipment,
    Skills,
    Traits,
    Inventory,
    System
}

public class PauseMenuState : StateMachine.State
{
    [SerializeField] protected PauseMenuStateType pauseMenuStateType;
    [SerializeField] protected GameObject defaultButton;
    protected GameObject lastButton;
    //[Header("External References")]
    //[Header("Events (Senders)")]
    [SerializeField] protected List<SignalListenerBase> signalListeners = new List<SignalListenerBase>();

    public virtual void Awake()
    {
        //state machine
        stateMachine = GetComponentInParent<StateMachine>();
        id = (int)pauseMenuStateType;

        //signal listeners
        signalListeners = GetComponentsInChildren<SignalListenerBase>().ToList();
        foreach (MonoBehaviour script in signalListeners)
        {
            script.enabled = false;
        } 
    }

    public override void OnEnter()
    {
        foreach (SignalListenerBase script in signalListeners)
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

    public virtual void ResetMemory()
    {
        lastButton = null;
    }
}
