using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[System.Serializable]
public enum BattleEventTrigger
{
    OnBattleStart,
    OnTurnStart,
    OnAttackTarget,
    OnHealTarget,
    OnKOTarget,
    OnDamaged,
    OnHealed,
    OnKOed,
    OnBattleEnd
}

[System.Serializable]
public class TriggerableSubEffect
{
    public BattleEventTrigger battleEventTrigger;
    public int chance;
    public SubEffect subEffect;
}

public class Trait : ScriptableObject
{
    public bool isUnlocked;
    public List<TriggerableSubEffect> triggerableSubEffects;

    public void TriggerEffects(Combatant combatant, Combatant target)
    {
        foreach(TriggerableSubEffect triggerableSubEffect in triggerableSubEffects)
        {
            float roll = Random.Range(1, 100);
            if(roll <= triggerableSubEffect.chance)
            {
                triggerableSubEffect.subEffect.TriggerEffect(combatant, target);
            }
        }
    }
}
