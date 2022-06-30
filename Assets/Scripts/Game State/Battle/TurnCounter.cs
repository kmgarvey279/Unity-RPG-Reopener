using System.Collections;
using System.Collections.Generic;
using UnityEngine;

//stores information related to combatant's place in turn queue
[System.Serializable]
public class TurnCounter
{
    public Combatant combatant;
    public float modifier = 0;
    [Header("Number of ticks remaining until next turn")]
    private float turnCost = 200f;
    public float counter = 0f;

    public TurnCounter(Combatant combatant)
    {
        this.combatant = combatant;
        ApplyTurnCost();
    }

    public void ApplyTurnCost()
    {
        counter += turnCost - combatant.battleStatDict[BattleStatType.Speed].GetValue();
    }

    // public float GetDefaultTurnCost()
    // {
    //     return turnCost - combatant.battleStatDict[BattleStatType.Speed].GetValue();
    // }

    public float GetValue()
    {
        // float speedBonus = combatant.battleStatDict[BattleStatType.Speed].GetValue();
        // float counterValue = counter - speedBonus + modifier;
        return counter;
    }


    public void ChangeModifier(float newModifier)
    {
        if(modifier + newModifier <= 1 && modifier + newModifier >= -1)
        {
            modifier += newModifier;
            counter = Mathf.Clamp(counter + modifier * combatant.battleStatDict[BattleStatType.Speed].GetBaseValue(), 0, 400);
        }
    }

    public void Tick()
    {
        if(counter > 0)
        {
            counter = counter - 1;
        }
    }
}
