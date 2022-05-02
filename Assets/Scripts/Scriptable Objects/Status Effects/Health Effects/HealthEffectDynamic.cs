using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class HealthEffectDynamic : HealthEffect
{
    public override int GetHealthChange(Combatant combatant, int potency, int remainingTurns)
    {
        List<float> dividers = new List<float> {2, 4, 8};
        int healthChange = 0;
        healthChange = Mathf.RoundToInt((float)potency / dividers[remainingTurns]);
        if(!isHeal)
        {
            healthChange = -healthChange;
        }
        return healthChange;
    }
}
