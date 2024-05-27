using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;
using UnityEngine.EventSystems;

public class PauseMenuMain : PauseSubmenu
{
    [SerializeField] private TextMeshProUGUI helpText;
    [Header("Tabs")]
    [SerializeField] private PauseMenuTab firstButton;
    //private Dictionary<PauseSubmenuType, PauseMenuTab> menuTabs = new Dictionary<PauseSubmenuType, PauseMenuTab>();
    //[SerializeField] private PauseMenuTab partyTab;
    //[SerializeField] private PauseMenuTab equipmentTab;
    //[SerializeField] private PauseMenuTab skillsTab;
    //[SerializeField] private PauseMenuTab traitsTab;
    //[SerializeField] private PauseMenuTab itemsTab;
    //[SerializeField] private PauseMenuTab systemTab;

    public void Update()
    {
        if (!isActive)
        {
            return;
        }

        if (Input.GetButtonDown("Cancel"))
        {
            Hide();
            pauseMenu.OnExitPauseMenu();
        }
    }

    public override void Display()
    {
        base.Display();

        EventSystem.current.SetSelectedGameObject(null);
        EventSystem.current.SetSelectedGameObject(firstButton.gameObject);
        firstButton.OnSelect(null);
    }
}
