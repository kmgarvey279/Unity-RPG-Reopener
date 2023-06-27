using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[System.Serializable]
public class Stat
{
    protected List<float> modifiers;
    public int BaseValue { get; protected set; }

    public Stat(int baseValue)
    {
        BaseValue = baseValue;
        modifiers = new List<float>();
    }

    public int GetValue()
    {
        float finalValue = BaseValue;
        modifiers.ForEach(x => finalValue *= x);
        return Mathf.FloorToInt(finalValue);
    }

    public virtual void AddModifier(float modifier)
    {
        modifiers.Add(modifier);
    }

    public virtual void RemoveModifier(float modifier)
    {
        modifiers.Remove(modifier);
    }
}
