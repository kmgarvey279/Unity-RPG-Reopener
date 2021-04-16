using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PauseScreen : MonoBehaviour
{
    [SerializeField] private GameData gameData;
    public GameObject mainMenuScreen;
    public GameObject battlePauseScreen;

    private void Update()
    {
        if(Input.GetButtonDown("Pause"))
        {
            TogglePause();
        }
    }

    private void TogglePause()
    {
        GameObject pauseScreen;
        if(gameData.gameState == GameState.Battle)
        {
            pauseScreen = battlePauseScreen;
        }
        else if(gameData.gameState == GameState.Normal)
        {
            pauseScreen = mainMenuScreen;
        }
        else
        {
            return;
        }

        if(!pauseScreen.activeInHierarchy)
        {
            pauseScreen.SetActive(true);
            Time.timeScale = 0f;
        }
        else 
        {
            pauseScreen.SetActive(false);
            Time.timeScale = 1f;
        }
    }
}
