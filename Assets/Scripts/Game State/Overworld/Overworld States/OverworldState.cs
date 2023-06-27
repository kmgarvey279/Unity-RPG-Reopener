using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using StateMachineNamespace;

public enum OverworldStateType
{
    FreeMove,
    Cutscene,
    Menu,
    Dialogue
}

public class OverworldState : StateMachine.State
{
    [SerializeField] protected OverworldStateType overworldStateType;
    [SerializeField] protected OverworldData overworldData;
    [Header("Signal Listeners")]
    public List<MonoBehaviour> signalListeners;

    public void Awake()
    {
        //state machine
        stateMachine = GetComponentInParent<StateMachine>(); 
        id = (int)overworldStateType;  
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
