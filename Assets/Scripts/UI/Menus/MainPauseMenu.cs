using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.EventSystems;

public class MainPauseMenu : MonoBehaviour
{
    public GameObject defaultMenu;
    public GameObject defaultButton;
    private GameObject activeMenu;

    private void OnEnable()
    {
        activeMenu = defaultMenu;
        EventSystem.current.SetSelectedGameObject(null);
        EventSystem.current.SetSelectedGameObject(defaultButton);
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
