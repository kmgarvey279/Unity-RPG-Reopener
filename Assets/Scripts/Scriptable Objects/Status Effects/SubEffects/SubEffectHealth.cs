using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SubEffectHealth : SubEffect
{
    [Header("Health Effects")]
    public bool isRegen;
    public float percentage; 
    public float bossPercentage;

    public override void TriggerEffect(Combatant combatant)
    {
        float percentageToUse = percentage;
        if(combatant is EnemyCombatant)
        {
            EnemyCombatant enemyCombatant = (EnemyCombatant)combatant;
            if(enemyCombatant.isBoss)
            {
                percentageToUse = bossPercentage;
            }
        }
        float potency = Mathf.Floor(combatant.hp.GetValue() / percentageToUse);
        if(isRegen) 
        {
            combatant.Heal(potency, false);
        }
        else
        {
            combatant.Damage(potency, false);
        }
    }
}

