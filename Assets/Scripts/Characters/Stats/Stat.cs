using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public enum StatType
{
    Health,
    Mana,
    Attack,
    Defense,
    Special
}

[System.Serializable]
public class Stat
{
    [SerializeField] protected float baseValue;
    public StatType statType;
    [SerializeField] protected List<float> modifiers = new List<float>();

    public float GetValue()
    {
        float finalValue = baseValue;
        modifiers.ForEach(x => finalValue += x);
        return finalValue;
    }

    public void ChangeValue(float amount)
    {
        baseValue = baseValue + amount;
    }

    public virtual void AddModifier(float modifier)
    {
        if(modifier != 0)
            modifiers.Add(modifier);
        
    }

    public virtual void RemoveModifier(float modifier)
    {
        if(modifier != 0)
            modifiers.Remove(modifier);
    }

    public virtual void RemoveAllModifiers()
    {
        modifiers.Clear();
    }
}
