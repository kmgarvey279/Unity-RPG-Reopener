using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[System.Serializable]
public class Stat
{
    [SerializeField] protected int baseValue;
    [SerializeField] protected List<int> modifiers = new List<int>();
    [SerializeField] protected List<float> multipliers = new List<float>();

    public Stat(int baseValue)
    {
        this.baseValue = baseValue;
    }

    public int GetValue()
    {
        int finalValue = baseValue;
        modifiers.ForEach(x => finalValue += x);
        foreach (float multiplier in multipliers)
        {
            finalValue = Mathf.RoundToInt((float)finalValue * multiplier);
        }
        return finalValue;
    }

    public void ChangeBaseValue(int amount)
    {
        baseValue = baseValue + amount;
    }

    public virtual void AddModifier(int modifier)
    {
        if(modifier != 0)
            modifiers.Add(modifier);
    }

    public virtual void AddMultiplier(float multiplier)
    {
        if(multiplier != 0)
            multipliers.Add(multiplier);
    }

    public virtual void RemoveModifier(int modifier)
    {
        if(modifier != 0)
            modifiers.Remove(modifier);
    }

    public virtual void RemoveMultiplier(float multiplier)
    {
        if(multiplier != 0)
            multipliers.Remove(multiplier);
    }

    public virtual void RemoveAllModifiers()
    {
        modifiers.Clear();
        multipliers.Clear();
    }
}
