using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.EventSystems;

public class PauseMenuStateMain : PauseMenuState
{
    [SerializeField] private GameObject display;
    [SerializeField] private SignalSender onExitPauseMenu;
    [SerializeField] private List<SelectableButton> sidebarButtons = new List<SelectableButton>();

    private void OnEnable()
    {
        InputManager.Instance.OnPressCancel.AddListener(Unpause);
        InputManager.Instance.OnPressUnpause.AddListener(Unpause);
    }

    private void OnDisable()
    {
        InputManager.Instance.OnPressCancel.RemoveListener(Unpause);
        InputManager.Instance.OnPressUnpause.RemoveListener(Unpause);
    }

    public override void Awake()
    {
        base.Awake();
        Hide();
    }

    public override void OnEnter()
    {
        base.OnEnter();
        Debug.Log("Now entering Main Pause Menu State");

        InputManager.Instance.OnPressCancel.AddListener(Unpause);
        InputManager.Instance.OnPressUnpause.AddListener(Unpause);

        Display();
    }

    public override void StateFixedUpdate()
    {
    }

    public override void OnExit()
    {
        base.OnExit();
        Hide();

        InputManager.Instance.OnPressCancel.RemoveListener(Unpause);
        InputManager.Instance.OnPressUnpause.RemoveListener(Unpause);

        foreach (SelectableButton button in sidebarButtons)
        {
            button.OnDeselect(null);
        }
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

    public void Unpause(bool isPressed)
    {
        stateMachine.ChangeState((int)PauseMenuStateType.Inactive);
    }

    public void OnClickSystem()
    {
        stateMachine.ChangeState((int)PauseMenuStateType.System);
    }

    public void OnClickParty()
    {
        stateMachine.ChangeState((int)PauseMenuStateType.Party);
    }

    public void OnClickEquipment()
    {
        stateMachine.ChangeState((int)PauseMenuStateType.Equipment);
    }

    public void OnClickSkills()
    {
        stateMachine.ChangeState((int)PauseMenuStateType.Skills);
    }

    public void OnClickTraits()
    {
        stateMachine.ChangeState((int)PauseMenuStateType.Traits);
    }

    public void OnClickInventory()
    {
        stateMachine.ChangeState((int)PauseMenuStateType.Inventory);
    }
}
