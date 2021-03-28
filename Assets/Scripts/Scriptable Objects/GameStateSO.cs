using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public enum GameState
{
    Active,
    Combat,
    Other
}

[CreateAssetMenu(fileName = "New Game State", menuName = "Game/Game State")]
public class GameStateSO : ScriptableObject
{
    public GameState gameState;

    private void OnEnable()
    {
        gameState = GameState.Active;
    }

    public void ChangeGameState(GameState newGameState)
    {
        gameState = newGameState;
    }
}
