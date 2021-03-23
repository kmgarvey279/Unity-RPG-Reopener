using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.EventSystems;

public class PauseMenus : MonoBehaviour
{
    public GameObject inventoryMenu;
    public GameObject characterMenu;
    // public GameObject optionsMenu;
    public GameObject[] menus;
    private int currentMenuIndex;

    private void Start()
    {
        menus = new GameObject[]{inventoryMenu, characterMenu};
        ChangeMenu(0);
    }

    private void Update()
    {
        if(Input.GetButtonDown("MenuLeft"))
        {
            int nextIndex = currentMenuIndex--;
            if(nextIndex < 0)
                nextIndex = menus.Length - 1;
            ChangeMenu(nextIndex);
        }
        else if(Input.GetButtonDown("MenuRight"))
        {
            int nextIndex = currentMenuIndex++;
            if(nextIndex > menus.Length - 1)
                nextIndex = 0;
            ChangeMenu(nextIndex);
        }
    }

    private void ChangeMenu(int newMenuIndex)
    {
        if(currentMenuIndex != null)
        {
            menus[currentMenuIndex].SetActive(false);
        }
        menus[newMenuIndex].SetActive(true);
        
        currentMenuIndex = newMenuIndex;
    }
}
