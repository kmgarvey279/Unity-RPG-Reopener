using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.EventSystems;
using System;
using JetBrains.Annotations;

public enum PauseSubmenuType
{
    Party,
    Equipment,
    Skills,
    Traits,
    Inventory,
    System
}

public class PauseMenu : MonoBehaviour
{
    [Header("Display")]
    [SerializeField] private GameObject display;
    //private bool isActive;

    [Header("Main Menu")]
    [SerializeField] private PauseMenuMain mainMenu;

    [Header("Submenus")]
    private Dictionary<PauseSubmenuType, PauseSubmenu> submenus = new Dictionary<PauseSubmenuType, PauseSubmenu>();
    [SerializeField] private PauseMenuParty partyMenu;
    [SerializeField] private PauseMenuEquip equipMenu;
    [SerializeField] private PauseMenuSkills skillsMenu;
    [SerializeField] private PauseMenuTraits traitsMenu;
    [SerializeField] private PauseMenuInventory itemMenu;
    [SerializeField] private PauseMenuSystem systemMenu;

    [SerializeField] private SignalSender onExitPauseMenu;


    public void Awake()
    {
        submenus.Add(PauseSubmenuType.Party, partyMenu);
        submenus.Add(PauseSubmenuType.Equipment, equipMenu);
        submenus.Add(PauseSubmenuType.Skills, skillsMenu);
        submenus.Add(PauseSubmenuType.Traits, traitsMenu);
        submenus.Add(PauseSubmenuType.Inventory, itemMenu);
        submenus.Add(PauseSubmenuType.System, systemMenu);

        if (display.activeInHierarchy)
        {
            display.SetActive(false);
        }
    }

    public void Update()
    {
        
    }

    public void Display()
    {
        if (!display.activeInHierarchy)
        {
            display.SetActive(true);
        }
        //isActive = true;

        mainMenu.Display();
    }

    public void Hide()
    {        
        //isActive = false;
        display.SetActive(false);
    }

    public void OnClickTab(PauseSubmenuType pauseSubmenuType)
    {
        mainMenu.Hide();
        mainMenu.gameObject.SetActive(false);

        submenus[pauseSubmenuType].Display();
    }

    public void OnExitSubmenu()
    {
        mainMenu.gameObject.SetActive(true);
        mainMenu.Display();
    }

    public void OnExitPauseMenu()
    {
        onExitPauseMenu.Raise();
    }
}
