using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu(fileName = "NewTriggerableEffect", menuName = "Triggerable Effects/Mana Effect/Percentage")]
public class TriggerableBattleEffectManaPercentage : TriggerableBattleEffect
{
    [SerializeField, Range(0.01f, 1f)] private float manaPercentage;
    [SerializeField] private bool useCurrentMP = false;

    public override void ApplyEffect(Combatant actor, Combatant target, ActionSummary actionSummary = null)
    {
        if (target.CombatantType != CombatantType.Player)
        {
            return;
        }
        PlayableCombatant playableCombatant = target as PlayableCombatant;
        float manaEffect = 0;

        float manaTotalToUse = playableCombatant.MP.GetValue();
        if (useCurrentMP)
        {
            manaTotalToUse = playableCombatant.MP.CurrentValue;
        }
        manaEffect = manaTotalToUse * manaPercentage;

        //final value
        int finalManaEffect = Mathf.Clamp(Mathf.FloorToInt(manaEffect), 1, 999);

        playableCombatant.OnChangeMana(finalManaEffect);
    }
}