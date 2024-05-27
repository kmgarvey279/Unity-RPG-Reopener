using Pathfinding;
using StateMachineNamespace;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PauseMenuManager : MonoBehaviour
{
    public static PauseMenuManager instance;

    [Header("State")]
    [SerializeField] private StateMachine stateMachine;
    private bool isPaused;

    //private void OnEnable()
    //{
    //    InputManager.onPause.AddListener(Pause);
    //    InputManager.onUnpause.AddListener(Unpause);
    //}

    //private void OnDisable()
    //{
    //    InputManager.onPause.RemoveListener(Pause);
    //    InputManager.onUnpause.RemoveListener(Unpause);
    //}

    private void Awake()
    {
        if (instance == null)
        {
            instance = this;
        }
    }

    //public void Update()
    //{
    //    //if (InputManager.InputLocked)
    //    //{
    //    //    return;
    //    //}

    //    //if (InputManager.OpenMenuInput && !isPaused)
    //    //{
    //    //    Pause();
    //    //}
    //    //else if (InputManager.CloseMenuInput && isPaused)
    //    //{
    //    //    Unpause();
    //    //}
    //}

    //public void Pause(bool isPressed)
    //{
    //    Debug.Log("Opening Pause Menu");
    //    isPaused = true;
        
    //    Time.timeScale = 0;
    //    InputManager.ChangeActionMap("UI");
        
    //    stateMachine.ChangeState((int)CommandMenuStateType.Main);
    //}

    //public void Unpause(bool isPressed)
    //{
    //    Debug.Log("Closing Pause Menu");
    //    isPaused = false;

    //    Time.timeScale = 1.0f;
    //    InputManager.ChangeActionMap("Overworld");
        
    //    stateMachine.ChangeState((int)CommandMenuStateType.Inactive);
    //}

    public void ResetMenus()
    {
        foreach (PauseMenuState pauseMenuState in stateMachine.states)
        {
            pauseMenuState.ResetMemory();
        }
    }
}
