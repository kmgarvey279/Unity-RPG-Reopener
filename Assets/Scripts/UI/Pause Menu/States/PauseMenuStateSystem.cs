using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.EventSystems;

public class PauseMenuStateSystem : PauseMenuState
{
    [SerializeField] private GameObject display;
    private enum SystemSubmenuType
    {
        Save,
        Sound,
        Controls
    }
    [SerializeField] private SystemSubmenuType currentSubmenu;
    [SerializeField] private GameObject saveSubmenu;
    [SerializeField] private GameObject soundSubmenu;
    [SerializeField] private GameObject controlsSubmenu;
    private Dictionary<SystemSubmenuType, GameObject> submenus;

    public override void Awake()
    {
        base.Awake();

        submenus = new Dictionary<SystemSubmenuType, GameObject>()
        {
            { SystemSubmenuType.Save, saveSubmenu },
            { SystemSubmenuType.Sound, soundSubmenu },
            { SystemSubmenuType.Controls, controlsSubmenu }
        };

        Hide();
    }

    public override void OnEnter()
    {
        base.OnEnter();
        Debug.Log("Now entering System Pause Menu State");
        
        InputManager.Instance.OnPressCancel.AddListener(PressCancel);

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

        InputManager.Instance.OnPressCancel.RemoveListener(PressCancel);

        Hide();
    }

    public void PressCancel(bool isPressed)
    {
        stateMachine.ChangeState((int)PauseMenuStateType.Main);
    }

    private void Display()
    {
        display.SetActive(true);

        DisplaySubmenu(SystemSubmenuType.Save);
    }


    private void DisplaySubmenu(SystemSubmenuType systemSubmenuType)
    {
        foreach (KeyValuePair<SystemSubmenuType, GameObject> submenu in submenus)
        {
            submenu.Value.SetActive(false);
        }

        currentSubmenu = systemSubmenuType;
        submenus[systemSubmenuType].SetActive(true);
    }

    private void Hide()
    {
        //foreach (KeyValuePair<SystemSubmenuType, GameObject> submenu in submenus)
        //{
        //    submenu.Value.SetActive(false);
        //}

        display.SetActive(false);
    }
}
