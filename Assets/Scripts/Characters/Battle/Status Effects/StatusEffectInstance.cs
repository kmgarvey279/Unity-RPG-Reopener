using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[System.Serializable]
public class StatusEffectInstance
{
    public StatusEffectSO statusEffectSO;
    public int turnCounter;
    // public float potency;

    public StatusEffectInstance(StatusEffectSO statusEffectSO)
    {
        this.statusEffectSO = statusEffectSO;
        this.turnCounter = statusEffectSO.turnDuration;
    }

    public void OnApply(Combatant combatant)
    {
        foreach(BattleStatModifier battleStatModifier in statusEffectSO.battleStatModifiers)
        {
            combatant.battleStatDict[battleStatModifier.statToModify].AddMultiplier(battleStatModifier.multiplier); 
        }
        foreach(ResistanceModifier resistanceModifier in statusEffectSO.resistanceModifiers)
        {
            combatant.resistDict[resistanceModifier.resistanceToModify].resistance += resistanceModifier.additive;  
        }
        foreach(SubEffect effect in statusEffectSO.onApplyEffects)
        {
            effect.TriggerEffect(combatant);
        }
    }

    public void OnRemove(Combatant combatant)
    {
        foreach(BattleStatModifier battleStatModifier in statusEffectSO.battleStatModifiers)
        {
            combatant.battleStatDict[battleStatModifier.statToModify].RemoveMultiplier(battleStatModifier.multiplier);  
        }
        foreach(ResistanceModifier resistanceModifier in statusEffectSO.resistanceModifiers)
        {
            combatant.resistDict[resistanceModifier.resistanceToModify].resistance -= resistanceModifier.additive;  
        }
        foreach(SubEffect effect in statusEffectSO.onRemoveEffects)
        {
            effect.TriggerEffect(combatant);
        }
    }

    public void OnTurnStart(Combatant combatant)
    {
        if(statusEffectSO.hasDuration)
        {
            turnCounter -= 1;
            if(turnCounter < 1)
            {
                combatant.RemoveStatusEffect(this);
            }
        }
        // foreach(SubEffect effect in statusEffectSO.onTurnStartEffects)
        // {
        //     effect.TriggerEffect(combatant);
        // }
    }

    public void OnTurnEnd(Combatant combatant)
    {   
        // if(statusEffectSO.healthEffect != null)
        // {
        //     statusEffectSO.healthEffect.Trigger(combatant, potency);
        //     potency = Mathf.FloorToInt((float)potency * statusEffectSO.healthEffect.turnMultiplier);
        // }
        foreach(TriggerableSubEffect effect in statusEffectSO.onTurnEndEffects)
        {
            effect.TriggerEffect(combatant);
        }
    }
}
