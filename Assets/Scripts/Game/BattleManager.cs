using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BattleManager : MonoBehaviour
{
    public GameData gameData;

    private bool escaping = false;
    private float escapeTimer;
    [SerializeField] private float escapeDuration;

    public PlayerParty playerParty;

    private void Start()
    {
        escapeTimer = escapeDuration;
    }

    private void Update()
    {
        if(escaping)
        {
            if(escapeTimer <= 0)
            {
                EndBattle();
            }
            else
            {
                escapeTimer--;
            }
        }
    }

    public void StartBattle()
    {
        gameData.ChangeGameState(GameState.Battle);
    }

    public void EndBattle()
    {
        escaping = false;
        escapeTimer = escapeDuration; 
        gameData.ChangeGameState(GameState.Normal);
    }

    public void OnAllyKO(GameObject target)
    {
        // foreach (GameObject enemy in enemies)
        // {
        //     Targeter targeter = enemy.GetComponent<Targeter>();
        //     targeter.RemoveTarget(target);
        // }
    }

    // public void OnAllyChange(GameObject oldAlly, GameObject newAlly)
    // {
    //     partyActive[oldAlly] = newAlly;
    //     partyReserve[newAlly] =
    // }

    //called when a new enemy joins battle (or first enemy triggers battle)
    public void OnEnemyEngage(GameObject newEnemy)
    {
        // //add enemy to battle manager enemy list
        // enemies.Add(newEnemy);
        // //add party to enemy target list
        // newEnemy.GetComponent<Targeter>().SetTargets(partyActive);
        // //add enemy to target list for each party member
        // foreach (GameObject character in partyActive)
        // {
        //     Targeter targeter = character.GetComponent<Targeter>();
        //     targeter.AddTarget(newEnemy);
        // }
        // //start battle (if not already to in battle state)
        // if(gameManager.gameState != GameState.Battle)
        // {
        //     StartBattle();
        // }
    }

    public void OnEnemyDie(GameObject target)
    {
        // //remove enemy from battle manager enemy list
        // enemies.Remove(target);
        // //end battle if it was the last enemy
        // if(enemies.Count <= 0)
        // {
        //     EndBattle();
        // }
        // else
        // {
        //     //remove enemy from each party member's target list
        //     foreach (GameObject character in partyActive)
        //     {
        //         Targeter targeter = character.GetComponent<Targeter>();
        //         targeter.RemoveTarget(target);
        //     }
        // }
    }

    public void StartEscape()
    {
       escaping = true; 
    }
}
