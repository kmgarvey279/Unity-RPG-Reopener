using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PauseScreen : MonoBehaviour
{
    [SerializeField] private GameData gameData;
    public GameObject menuScreen;

    private void Update()
    {
        if(Input.GetButtonDown("Pause"))
        {
            TogglePause();
        }
    }

    private void TogglePause()
    {
        if(!menuScreen.activeInHierarchy)
        {
            menuScreen.SetActive(true);
            Time.timeScale = 0f;
        }
        else 
        {
            menuScreen.SetActive(false);
            Time.timeScale = 1f;
        }
    }
}
