using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using StateMachineNamespace;
using UnityEngine.EventSystems;

public class TitleMenuStateMain : TitleMenuState
{
    [SerializeField] private GameObject display;
    [SerializeField] private SceneField firstScene;
    [SerializeField] private GameObject loadButton;
    [SerializeField] private GameObject optionsButton;

    //private void OnEnable()
    //{
    //    InputManager.onPressCancel.AddListener(OnCancel);
    //}

    //private void OnDisable()
    //{
    //    InputManager.onPressCancel.RemoveListener(OnCancel);
    //}

    public override void OnEnter()
    {
        base.OnEnter();

        Display();
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

        Hide();
    }

    private void Display()
    {
        display.SetActive(true);

        if (lastButton != null)
        {
            EventSystem.current.SetSelectedGameObject(null);
            EventSystem.current.SetSelectedGameObject(lastButton);
        }
        else
        {
            EventSystem.current.SetSelectedGameObject(null);
            EventSystem.current.SetSelectedGameObject(defaultButton);
        }
    }

    private void Hide()
    {
        display.SetActive(false);
    }

    //public void OnCancel(bool isPressed)
    //{
    //    lastButton = defaultButton;
    //    stateMachine.ChangeState((int)TitleMenuStateType.Load);
    //}

    public void OnClickNewGame()
    {
        SaveManager.Instance.StartNewGame();

        SceneSetupManager sceneSetupManager = FindObjectOfType<SceneSetupManager>();
        if (sceneSetupManager)
        {
            StartCoroutine(sceneSetupManager.OnExitSceneCo(firstScene));
        }
        else
        {
            Debug.Log("Setup manager not found!");
        }
    }

    public void OnClickLoad()
    {
        lastButton = loadButton;
        stateMachine.ChangeState((int)TitleMenuStateType.Load);
    }

    public void OnClickOptions()
    {
        lastButton = optionsButton;
        stateMachine.ChangeState((int)TitleMenuStateType.Options);
    }

    public void OnClickExit()
    {
    }
}
