using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SubEffectMana : SubEffect
{
    [Header("Health Effects")]
    public bool isRegen;
    public float percentage;

    public override void TriggerEffect(Combatant combatant)
    {
        if(combatant is PlayableCombatant)
        {
            PlayableCombatant playableCombatant = (PlayableCombatant) combatant;
            float potency = Mathf.Floor(playableCombatant.mp.GetValue() / percentage);
            if(!isRegen) 
            {
                potency = -potency;
            }
            playableCombatant.ChangeMana(potency);
        }
    }
}
