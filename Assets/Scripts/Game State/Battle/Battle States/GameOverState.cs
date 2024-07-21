using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class GameOverState : BattleState
{
    [SerializeField] private GameObject gameOverScreen;
    public override void OnEnter()
    {
        Debug.Log("Entering Game Over State");

        base.OnEnter();

        battleManager.ToggleCanQueueInterventions(false);
        gameOverScreen.SetActive(true);
    }

    public override void StateUpdate()
    {
    }

    public override void StateFixedUpdate()
    {

    }

    public override void OnExit()
    {
        base.OnExit();
    }
}
