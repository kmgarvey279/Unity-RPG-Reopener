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
            public string name;
            [HideInInspector]
            public string nextState;
            //initialize variables
            public abstract void OnEnter();
            //perform logic
            public abstract void StateUpdate();
            public abstract void StateFixedUpdate();
            //check if state should be changed
            public abstract string CheckConditions();
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
                string stateId = currentState.CheckConditions();
                if(stateId.Length > 0)
                {
                    foreach (State state in states)
                    {
                        if(state.name == stateId)  
                        {
                            currentState.OnExit();
                            currentState = state;
                            currentState.OnEnter();
                            break;
                        }  
                    }
                }
            }
        }

        void FixedUpdate()
        {
            if(isActive == true && currentState != null)
            {
                currentState.StateFixedUpdate();
            }
        }

    //change state outside of normal logic (ex: player dies)
        public virtual void ChangeState(State state)
        {
            if(state == null)
                return;
            if(currentState != null)
            {
                currentState.OnExit();
            }
            currentState = state;
            currentState.OnEnter();
        }

        public void SetActive(bool isActive)
        {
            this.isActive = isActive;
        }
    }
}

