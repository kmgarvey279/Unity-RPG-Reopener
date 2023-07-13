using Mono.Cecil;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[System.Serializable]
public class ModifierCollection
{
    private Dictionary<ModifierType, List<float>> modifierDict = new Dictionary<ModifierType, List<float>>();

    public ModifierCollection()
    {
        modifierDict.Add(ModifierType.Addend, new List<float>());
        modifierDict.Add(ModifierType.Multiplier, new List<float>());
    }

    public void AddModifier(float modifier, ModifierType modifierType)
    {
        modifierDict[modifierType].Add(modifier);
    }

    public void RemoveModifier(float modifier, ModifierType modifierType)
    {
        if (modifierDict[modifierType].Contains(modifier))
        {
            modifierDict[modifierType].Remove(modifier);
        }
    }

    public float GetModifiedValue(float baseValue)
    {
        float modifiedValue = baseValue;
        foreach (float multiplier in modifierDict[ModifierType.Multiplier])
        {
            modifiedValue += modifiedValue * multiplier;
        }
        foreach (float addend in modifierDict[ModifierType.Addend])
        {
            modifiedValue += addend;
        }
        return modifiedValue;
    }
}


[System.Serializable]
public class Stat
{
    protected ModifierCollection modifierCollection;
    public int BaseValue { get; protected set; }
    public int CurrentValue { get; protected set; }

    public Stat(int baseValue)
    {
        BaseValue = baseValue;
        CurrentValue = baseValue;
        modifierCollection = new ModifierCollection();
    }

    public void UpdateBaseValue(int newBaseValue)
    {
        BaseValue = newBaseValue;
        UpdateCurrentValue();
    }

    public void UpdateCurrentValue()
    {
       CurrentValue = Mathf.FloorToInt(modifierCollection.GetModifiedValue(BaseValue));
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
