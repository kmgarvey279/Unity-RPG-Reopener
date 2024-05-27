using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu(fileName = "NewTriggerableEffect", menuName = "Triggerable Effects/Health Effect/Heal")]

public class TriggerableEffectHeal : TriggerableEffect
{
    [Header("Triggerable Effect Heal")]
    private const float powerMultiplierMin = 0.1f;
    private const float powerMultiplierMax = 10f;

    public override void ApplyEffect(Combatant actor, Combatant target, float value)
    {
        Debug.Log("Triggering Heal Secondary Effect");

        //clamp value
        value = Mathf.Clamp(value, powerMultiplierMin, powerMultiplierMax);

        //get base power
        float healAmount = value * actor.Stats[IntStatType.Healing] * BattleConsts.HealingMultiplierConst;
        Debug.Log("Base Heal Secondary Effect: " + healAmount);

        //universal modifiers
        foreach (float modifier in actor.UniversalModifiers[BattleEventType.Acting][UniversalModifierType.Healing])
        {
            healAmount *= modifier;
        }
        foreach (float modifier in target.UniversalModifiers[BattleEventType.Targeted][UniversalModifierType.Healing])
        {
            healAmount *= modifier;
        }
        Debug.Log("Heal Secondary Effect after mods: " + healAmount);

        //rng variance
        healAmount *= Random.Range(BattleConsts.VarianceMinHeal, BattleConsts.VarianceMaxHeal);
        Debug.Log("Heal Secondary Effect after RNG: " + healAmount);

        //final heal
        int intHeal = Mathf.Clamp(Mathf.CeilToInt(healAmount), 1, 9999);
        Debug.Log("Final Heal Secondary Effect: " + intHeal);

        //final value
        target.OnHealed(intHeal, false);
    }
}
