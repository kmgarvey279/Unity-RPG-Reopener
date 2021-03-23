using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PauseScreen : MonoBehaviour
{
    public GameObject menus;

    private void Update()
    {
        if(Input.GetButtonDown("Pause"))
        {
            TogglePause();
        }
    }

    private void TogglePause()
    {
        if(!menus.activeInHierarchy)
        {
            menus.SetActive(true);
            Time.timeScale = 0f;
        }
        else 
        {
            menus.SetActive(false);
            Time.timeScale = 1f;
        }
    }
}
