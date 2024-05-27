using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace StateMachineNamespace
{
    [System.Serializable]
    public class StateMachine: MonoBehaviour
    {
        [System.Serializable]
        public abstract class State: MonoBehaviour
        {
            [HideInInspector]
            public StateMachine stateMachine;
            [HideInInspector]
            public int id;

            //initialize variables
            public abstract void OnEnter();
            //perform logic/physics
            public abstract void StateUpdate();
            public abstract void StateFixedUpdate();
            //clean up
            public abstract void OnExit();
        } 

        public State[] states;
        public State currentState;
        //pause
        public bool isActive = true;

        public void Start()
        {
            if(currentState != null)
            {
                currentState.OnEnter();
            }
        }

        public void Update()
        {
            if(isActive == true && currentState != null)
            {
                currentState.StateUpdate();
            }
        }

        public void FixedUpdate()
        {
            if(isActive == true && currentState != null)
            {
                currentState.StateFixedUpdate();
            }
        }

        public void ChangeState(int nextStateId)
        {
            foreach (State state in states)
            {
                if(state.id == nextStateId)  
                {
                    currentState.OnExit();
                    StartCoroutine(ChangeStateCo(state));
                    break;
                }  
            }
        }

        public IEnumerator ChangeStateCo(State state)
        {
            yield return null;
            currentState = state;
            currentState.OnEnter();
        }

        public void SetActive(bool isActive)
        {
            this.isActive = isActive;
        }
    }
}

