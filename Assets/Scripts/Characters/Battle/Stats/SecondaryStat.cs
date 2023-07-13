using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SecondaryStat
{
    protected ModifierCollection modifierCollection;
    public float BaseValue { get; protected set; }
    public float CurrentValue { get; protected set; }

    public SecondaryStat(float baseValue)
    {
        BaseValue = baseValue;
        CurrentValue = baseValue;
        modifierCollection = new ModifierCollection();
    }

    public void UpdateBaseValue(float newBaseValue)
    {
        BaseValue = newBaseValue;
        UpdateCurrentValue();
    }

    public void UpdateCurrentValue()
    {
        CurrentValue = modifierCollection.GetModifiedValue(BaseValue);
    }

    public virtual void AddModifier(float modifier, ModifierType modifierType)
    {
        modifierCollection.AddModifier(modifier, modifierType);
        UpdateCurrentValue();
    }

    public virtual void RemoveModifier(float modifier, ModifierType modifierType)
    {
        modifierCollection.RemoveModifier(modifier, modifierType);
        UpdateCurrentValue();
    }
}
