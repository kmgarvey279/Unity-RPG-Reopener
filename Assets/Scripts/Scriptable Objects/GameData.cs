using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public enum GameState
{
    Move,
    Battle
}

[CreateAssetMenu(fileName = "New Game Data", menuName = "Game Data")]
public class GameData: ScriptableObject
{
    public GameState gameState;

    private void Start()
    {
        gameState = GameState.Move;
    }

    public void ChangeGameState(GameState newGameState)
    {
        gameState = newGameState;
    }
}
