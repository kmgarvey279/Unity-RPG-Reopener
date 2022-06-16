using System.Collections;
using System.Collections.Generic;
using UnityEngine;

//stores information related to combatant's place in turn queue
[System.Serializable]
public class TurnCounter
{
    public Combatant combatant;
    [Header("Number of ticks remaining until next turn")]
    private int turnCost = 200;
    private int counter = 0;

    public TurnCounter(Combatant combatant)
    {
        this.combatant = combatant;
    }

    public void Reset()
    {
        counter = turnCost;
    }

    public int GetValue()
    {
        int speedBonus = Mathf.FloorToInt((float)combatant.battleStatDict[BattleStatType.Speed].GetValue());
        int counterValue = counter - speedBonus;
        return Mathf.Clamp(counterValue, 0, 999);
    }

    public void ChangeValue(int amount)
    {
        counter += amount;
    }

    public void Tick()
    {
        if(counter > 0)
        {
            counter = counter - 1;
        }
    }
}
