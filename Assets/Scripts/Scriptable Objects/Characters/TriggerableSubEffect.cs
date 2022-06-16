using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[System.Serializable]
public enum BattleEventTrigger
{
    OnBattleStart,
    OnTurnStart,
    OnTurnEnd,
    OnBattleEnd,
    OnCompleteAction,
    OnDamaged,
    OnHealed,
    OnKO
}

[System.Serializable]
public class TriggerableSubEffect
{
    // public BattleEventTrigger battleEventTrigger;
    [Range(1, 100)] public float chance;
    public SubEffect subEffect;

    public void TriggerEffect(Combatant combatant)
    {
        if(Roll(chance))
        {
            subEffect.TriggerEffect(combatant);
        }
    }

    public bool Roll(float chance)
    {
        float roll = Random.Range(1, 100);
        if(roll <= chance)
        {
            return true;
        }
        else
        {
            return false;
        }
    }
}
