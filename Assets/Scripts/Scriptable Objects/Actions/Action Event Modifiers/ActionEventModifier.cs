using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public enum EventTriggerType
{
    OnAct,
    OnTargeted,
    OnPartyTargeted,
    OnApplyActionEffects
}

public enum ActionModifierType
{
    ActionPowerPhysical,
    ActionPowerMagic,
    ActionPowerHeal,
    TargetDefensePhysical,
    TargetDefenseMagic,
    HitRate,
    CritRate,
    CritPower,
    EffectRate,
    FinalPowerPhysical,
    FinalPowerMagic
}

public class ActionModifier
{
    public ActionModifierType actionModifierType;
    public float additive;
    public float multiplier;
}

public class ActionEventModifier : ScriptableObject
{
    public string modifierName;
    [Range(1f, 100f)] public float chance = 100f;
    public EventTriggerType eventTriggerType;
    public List<ActionModifier> actionModifiers = new List<ActionModifier>();
    public string targetAnimatorOverride = "";
    public bool removeOnApply;

    public virtual ActionEvent ApplyModifiers(ActionEvent actionEvent, Combatant combatant)
    {
        if(!Roll(chance))
        {
            foreach(ActionModifier modifier in actionModifiers)
            {
                actionEvent.actionModifiers[modifier.actionModifierType].Add(modifier);            
            }
        }
        return actionEvent;
    }

    public bool Roll(float chance)
    {
        int roll = Random.Range(1, 100);
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
