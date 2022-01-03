using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SubEffectAddStatus : SubEffect
{
    public StatusEffectSO statusEffectSO;
    public bool addToSelf;
    public bool addToOther;

    public override void TriggerEffect(Combatant thisCombatant, Combatant otherCombatant)
    {
        int userPower = thisCombatant.level + 5;
        if(addToSelf)
        {
            thisCombatant.AddStatusEffect(statusEffectSO, userPower);
        }
        else if(addToOther)
        {
            otherCombatant.AddStatusEffect(statusEffectSO, userPower);
        }
    }
}
