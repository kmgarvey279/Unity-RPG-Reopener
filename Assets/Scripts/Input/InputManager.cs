using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Events;
using UnityEngine.InputSystem;

public class Vector2Event : UnityEvent<Vector2>
{
}
public class BoolEvent : UnityEvent<bool>
{
}

public class InputManager : MonoBehaviour
{
    public static InputManager Instance;

    public PlayerInput PlayerInput;
    [SerializeField] public bool InputLocked;
    [SerializeField] private string actionMap;
    
    //overworld
    public Vector2Event OnPressMove { get; private set; } = new Vector2Event();
    public BoolEvent OnPressInteract { get; private set; } = new BoolEvent();
    public BoolEvent OnPressRun { get; private set; } = new BoolEvent();
    public BoolEvent OnPressPause { get; private set; } = new BoolEvent();

    //dialogue
    public BoolEvent OnPressAdvance { get; private set; } = new BoolEvent();

    //menu
    public BoolEvent OnPressUnpause { get; private set; } = new BoolEvent();
    public BoolEvent OnPressSubmit { get; private set; } = new BoolEvent();
    public BoolEvent OnPressCancel { get; private set; } = new BoolEvent();
    public BoolEvent OnPressRight { get; private set; } = new BoolEvent();
    public BoolEvent OnPressLeft { get; private set; } = new BoolEvent();

    //battle
    //public static BoolEvent onPressSubmitBattle = new BoolEvent();
    //public static BoolEvent onPressCancelBattle = new BoolEvent();
    public BoolEvent OnPressTabUI { get; private set; } = new BoolEvent();
    public BoolEvent OnPressTabTarget { get; private set; } = new BoolEvent();
    public BoolEvent OnPressIntervention1 { get; private set; } = new BoolEvent();
    public BoolEvent OnPressIntervention2 { get; private set; } = new BoolEvent();
    public BoolEvent OnPressIntervention3 { get; private set; } = new BoolEvent();

    private void Awake()
    {
        if (Instance == null)
        {
            Instance = this;
        }
        PlayerInput = GetComponent<PlayerInput>();
    }

    public void OnMove(InputValue value)
    {
        OnPressMove.Invoke(value.Get<Vector2>());
    }

    public void OnInteract(InputValue value)
    {
        OnPressInteract.Invoke(value.isPressed);
    }

    public void OnPause(InputValue value)
    {
        OnPressPause.Invoke(value.isPressed);
    }

    public void OnUnpause(InputValue value)
    {
        OnPressUnpause.Invoke(value.isPressed);
    }

    public void OnSubmit(InputValue value)
    {
        OnPressSubmit.Invoke(value.isPressed);
    }

    public void OnCancel(InputValue value)
    {
        OnPressCancel.Invoke(value.isPressed);
    }

    public void OnLeft(InputValue value)
    {
        OnPressLeft.Invoke(value.isPressed);
    }

    public void OnRight(InputValue value)
    {
        OnPressRight.Invoke(value.isPressed);
    }

    public void OnAdvance(InputValue value)
    {
        OnPressAdvance.Invoke(value.isPressed);
    }

    public void OnRun(InputValue value)
    {
        OnPressRun.Invoke(value.isPressed);
    }

    #region Battle
    //public void OnSubmitBattle(InputValue value)
    //{
    //    onPressSubmitBattle.Invoke(value.isPressed);
    //}

    //public void OnCancelBattle(InputValue value)
    //{
    //    onPressCancelBattle.Invoke(value.isPressed);
    //}

    public void OnTabUI(InputValue value)
    {
        OnPressTabUI.Invoke(value.isPressed);
    }

    public void OnTabTarget(InputValue value)
    {
        OnPressTabTarget.Invoke(value.isPressed);
    }

    public void OnTriggerIntervention1(InputValue value)
    {
        OnPressIntervention1.Invoke(value.isPressed);
    }

    public void OnTriggerIntervention2(InputValue value)
    {
        OnPressIntervention2.Invoke(value.isPressed);
    }

    public void OnTriggerIntervention3(InputValue value)
    {
        OnPressIntervention3.Invoke(value.isPressed);
    }
    #endregion

    public void LockInput()
    {
        Instance.InputLocked = true;
    }

    public void UnlockInput()
    {
        InputLocked = false;
    }

    public void ChangeActionMap(string newActionMap)
    {
        PlayerInput.SwitchCurrentActionMap(newActionMap);
        if (PlayerInput.currentActionMap.name == newActionMap)
        {
            actionMap = PlayerInput.currentActionMap.name;
        }
        else
        {
            Debug.LogError("Invalid action map string");
        }
        Debug.Log("Current Action Map:" + actionMap);
    }
}
