using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SubEffectRemoveStatus : SubEffect
{
    public bool removeBuffs;
    public bool removeDebuffs;
    public StatusType statusTypeToRemove;

    public override void TriggerEffect(Combatant user, Combatant target)
    {
        if(target.statusEffects.Count > 0)
        {
            foreach(StatusEffectInstance statusEffectInstance in target.statusEffects)
            {
                if((removeDebuffs && !statusEffectInstance.statusEffectSO.isBuff || removeBuffs && statusEffectInstance.statusEffectSO.isBuff) && statusEffectInstance.statusEffectSO.canRemove && statusEffectInstance.statusEffectSO.statusType == statusTypeToRemove)
                {
                    target.RemoveStatusEffect(statusEffectInstance);
                }
            }
        }
    }
}
