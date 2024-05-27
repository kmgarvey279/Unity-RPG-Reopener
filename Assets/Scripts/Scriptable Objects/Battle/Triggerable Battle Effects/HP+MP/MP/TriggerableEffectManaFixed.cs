using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu(fileName = "NewTriggerableEffect", menuName = "Triggerable Effects/Mana Effect/Fixed")]
public class TriggerableEffectManaFixed : TriggerableEffect
{
    public override void ApplyEffect(Combatant actor, Combatant target, float value)
    {
        if (target.CombatantType != CombatantType.Player)
        {
            return;
        }

        int intValue = Mathf.Clamp(Mathf.FloorToInt(value), -999, 999);
        PlayableCombatant playableCombatant = target as PlayableCombatant;
        if (intValue > 0)
        {
            playableCombatant.OnGainMana(intValue);
        }
        else if (intValue < 0)
        {
            playableCombatant.OnSpendMana(Mathf.Abs(intValue));
        }
    }
}
