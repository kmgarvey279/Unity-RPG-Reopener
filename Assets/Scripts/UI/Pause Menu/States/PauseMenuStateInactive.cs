using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UIElements;

public class PauseMenuStateInactive : PauseMenuState
{
    [SerializeField] private GameObject background;

    public override void OnEnter()
    {
        base.OnEnter();
        Debug.Log("Now entering Pause Menu Inactive State");

        background.SetActive(false);

        Time.timeScale = 1.0f;
        InputManager.Instance.ChangeActionMap("Overworld");

        InputManager.Instance.OnPressPause.AddListener(Pause);
    }

    public override void StateUpdate()
    {
    }

    public override void StateFixedUpdate()
    {
    }

    public override void OnExit()
    {
        base.OnExit();
        Debug.Log("Now exiting Pause Menu Inactive State");

        background.SetActive(true);

        Time.timeScale = 0;
        InputManager.Instance.ChangeActionMap("UI");

        InputManager.Instance.OnPressPause.RemoveListener(Pause);
    }

    public void Pause(bool isPressed)
    {
        stateMachine.ChangeState((int)PauseMenuStateType.Main);
    }
}
