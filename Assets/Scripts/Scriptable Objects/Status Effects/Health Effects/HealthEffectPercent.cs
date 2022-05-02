using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class HealthEffectPercent : HealthEffect
{
    public override int GetHealthChange(Combatant combatant, int potency, int remainingTurns)
    {
        int healthChange = 0;
        if(combatant is EnemyCombatant)
        {
            EnemyCombatant enemyCombatant = (EnemyCombatant)combatant;
            if(enemyCombatant.isBoss)
            {
                healthChange = Mathf.RoundToInt((float)combatant.hp.GetValue() % 5f);
            }
            else
            {
                healthChange = Mathf.RoundToInt((float)combatant.hp.GetValue() % 10f);
            }
            if(!isHeal)
            {
                healthChange = -healthChange;
            }
        }
        return healthChange;
    }
}
