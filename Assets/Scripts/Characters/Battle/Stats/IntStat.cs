using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public enum IntStatType
{
    MaxHP,
    MaxMP,
    Attack,
    MAttack,
    Defense,
    MDefense,
    Healing,
    Agility,
    CritRate,
    EvadeRate
}

[System.Serializable]
public class ModifierCollection
{
    public Dictionary<ValueModifierType, List<float>> ModifierDict { get; private set; } = new Dictionary<ValueModifierType, List<float>>();

    //public ModifierCollection()
    //{
    //    ModifierDict.Add(ModifierType.Addend, new List<float>());
    //    ModifierDict.Add(ModifierType.Multiplier, new List<float>());
    //}

    //public void AddModifier(float modifier, ModifierType modifierType)
    //{
    //    ModifierDict[modifierType].Add(modifier);
    //}

    //public void RemoveModifier(float modifier, ModifierType modifierType)
    //{
    //    if (ModifierDict[modifierType].Contains(modifier))
    //    {
    //        ModifierDict[modifierType].Remove(modifier);
    //    }
    //}

    //public float GetModifiedValue(float baseValue)
    //{
    //    float modifiedValue = baseValue;
    //    foreach (float multiplier in ModifierDict[ModifierType.Multiplier])
    //    {
    //        modifiedValue += modifiedValue * multiplier;
    //    }
    //    foreach (float addend in ModifierDict[ModifierType.Addend])
    //    {
    //        modifiedValue += addend;
    //    }
    //    return modifiedValue;
    //}
}


[System.Serializable]
public class IntStat
{
    protected ModifierCollection modifierCollection;
    public int BaseValue { get; protected set; }
    public int CurrentValue { get; protected set; }

    public IntStat(int baseValue)
    {
        BaseValue = baseValue;
        CurrentValue = baseValue;
        modifierCollection = new ModifierCollection();
    }

    //public IntStat Clone()
    //{
    //    IntStat copy = new IntStat(BaseValue);
    //    foreach (KeyValuePair<ModifierType, List<float>> entry in modifierCollection.ModifierDict)
    //    {
    //        foreach (float modifier in entry.Value)
    //        {
    //            copy.AddModifier(modifier, entry.Key);
    //        }
    //    }
    //    return copy;
    //}

    //public void UpdateBaseValue(int newBaseValue)
    //{
    //    BaseValue = newBaseValue;
    //    UpdateCurrentValue();
    //}

    //public void UpdateCurrentValue()
    //{
    //   CurrentValue = Mathf.FloorToInt(modifierCollection.GetModifiedValue(BaseValue));
    //}

    //public virtual void AddModifier(float modifier, ModifierType modifierType)
    //{
    //    modifierCollection.AddModifier(modifier, modifierType);
    //    UpdateCurrentValue();
    //}

    //public virtual void RemoveModifier(float modifier, ModifierType modifierType)
    //{
    //    modifierCollection.RemoveModifier(modifier, modifierType);
    //    UpdateCurrentValue();
    //}
}
