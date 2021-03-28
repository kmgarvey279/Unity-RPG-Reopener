using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PauseScreen : MonoBehaviour
{
    [SerializeField] private GameStateSO gameStateSO;
    public GameObject mainMenuScreen;
    public GameObject combatPauseScreen;

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
        if(gameStateSO.gameState == GameState.Combat)
        {
            pauseScreen = combatPauseScreen;
        }
        else if(gameStateSO.gameState == GameState.Active)
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
