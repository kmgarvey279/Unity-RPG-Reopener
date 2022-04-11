using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.EventSystems;

public class MainPauseMenu : MonoBehaviour
{
    [SerializeField] private GameObject display;
    [SerializeField] private GameObject defaultMenu;
    [SerializeField] private GameObject defaultButton;
    private GameObject activeMenu;

    private void OnEnable()
    {
        activeMenu = defaultMenu;
        EventSystem.current.SetSelectedGameObject(null);
        EventSystem.current.SetSelectedGameObject(defaultButton);
    }

    public void Display()
    {
        display.SetActive(true);
    }

    public void Hide()
    {
        display.SetActive(false);
    }

    public void ChangeMenu(GameObject newMenu)
    {
        if(newMenu != activeMenu)
        {
            activeMenu.SetActive(false);
            newMenu.SetActive(true);
            activeMenu = newMenu;
        }
    }
}
