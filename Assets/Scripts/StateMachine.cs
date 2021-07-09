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

        void Start()
        {
            if(currentState != null)
            {
                currentState.OnEnter();
            }
        }

        void Update()
        {
            if(isActive == true && currentState != null)
            {
                currentState.StateUpdate();
            }
        }

        void FixedUpdate()
        {
            if(isActive == true && currentState != null)
            {
                currentState.StateFixedUpdate();
            }
        }

        public void ChangeState(int nextStateId)
        {
            foreach(State state in states)
            {
                if(state.id == nextStateId)  
                {
                    currentState.OnExit();
                    currentState = state;
                    currentState.OnEnter();
                    break;
                }  
            }
            
        }

        public void SetActive(bool isActive)
        {
            this.isActive = isActive;
        }
    }
}

