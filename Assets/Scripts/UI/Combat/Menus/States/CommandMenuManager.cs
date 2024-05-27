using StateMachineNamespace;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CommandMenuManager : MonoBehaviour
{
    //[SerializeField] private GameObject display;

    [Header("State")]
    [SerializeField] private StateMachine stateMachine;
    
    private void OnEnable()
    {
        Hide();
    }

    public void Display()
    {
        //display.SetActive(true);
        if (stateMachine.currentState.id == (int)CommandMenuStateType.Main || stateMachine.currentState.id == (int)CommandMenuStateType.Inactive)
        {
            stateMachine.ChangeState((int)CommandMenuStateType.Main);
        }
    }

    public void Hide()
    {
        stateMachine.ChangeState((int)CommandMenuStateType.Inactive);
        ResetMenu();
    }

    public void ResetMenu()
    {
        Debug.Log("Resetting menu");
        //lastMenu = CommandMenuStateType.Main;
        //onChangeCommandMenuState.Raise((int)CommandMenuStateType.Inactive);
        foreach (CommandMenuState commandMenuState in stateMachine.states)
        {
            commandMenuState.Reset();
        }
    }
}
