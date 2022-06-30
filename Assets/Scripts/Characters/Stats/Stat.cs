using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[System.Serializable]
public class Stat
{
    [SerializeField] protected float baseValue;
    [SerializeField] protected List<float> additives = new List<float>();
    [SerializeField] protected List<float> multipliers = new List<float>();

    public Stat(float baseValue)
    {
        this.baseValue = baseValue;
    }

    public float GetBaseValue()
    {
        return baseValue;
    }

    public float GetValue()
    {
        float finalValue = baseValue;
        additives.ForEach(x => finalValue += x);
        multipliers.ForEach(x => finalValue *= x);
        return finalValue;
    }

    public void ChangeBaseValue(int amount)
    {
        baseValue = baseValue + amount;
    }

    public virtual void AddAdditive(float additive)
    {
        additives.Add(additive);
    }

    public virtual void AddMultiplier(float multiplier)
    {
        multipliers.Add(multiplier);
    }

    public virtual void RemoveAdditive(float additive)
    {
        additives.Remove(additive);
    }

    public virtual void RemoveMultiplier(float multiplier)
    {
        multipliers.Remove(multiplier);
    }

    public virtual void RemoveAllModifiers()
    {
        additives.Clear();
        multipliers.Clear();
    }
}
