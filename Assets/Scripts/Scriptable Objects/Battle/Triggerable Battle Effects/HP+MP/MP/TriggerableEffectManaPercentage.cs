using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.InputSystem;

[CreateAssetMenu(fileName = "NewTriggerableEffect", menuName = "Triggerable Effects/Mana Effect/Percentage")]
public class TriggerableEffectManaPercentage : TriggerableEffect
{
    public override void ApplyEffect(Combatant actor, Combatant target, float value)
    {
        if (target.CombatantType != CombatantType.Player)
        {
            return;
        }

        value = Mathf.Clamp01(value);
        PlayableCombatant playableCombatant = target as PlayableCombatant;
        float manaEffect = 0;
        manaEffect = playableCombatant.MaxMP * value;

        //final value
        int intValue = Mathf.Clamp(Mathf.FloorToInt(manaEffect), -999, 999);
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