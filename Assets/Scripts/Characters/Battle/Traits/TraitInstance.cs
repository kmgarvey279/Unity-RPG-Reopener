using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class TraitInstance
{
    public Combatant combatant;
    public Trait trait;
    public int useCount;
    public TraitInstance(Combatant combatant, Trait trait)
    {
        this.combatant = combatant;
        this.trait = trait;
    }

    public void TriggerEffects()
    {
        foreach(TriggerableSubEffect triggerableSubEffect in trait.triggerableSubEffects)
        {
            float roll = Random.Range(1, 100);
            if(roll <= triggerableSubEffect.chance)
            {
                triggerableSubEffect.subEffect.TriggerEffect(combatant);
            }
        }
    }
}
