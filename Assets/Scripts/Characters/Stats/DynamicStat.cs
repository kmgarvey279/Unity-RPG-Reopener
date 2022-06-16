using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[System.Serializable]
public class DynamicStat : Stat
{
    public float currentValue;

    public DynamicStat(float baseValue, float currentValue) : base(baseValue)
    {
        this.baseValue = baseValue;
        this.currentValue = currentValue;
    }

    public override void AddAdditive(float additive)
    {
        additives.Add(additive);     
        currentValue = Mathf.Clamp(currentValue, 0, GetValue());
    }

    public override void RemoveAdditive(float additive)
    {
        additives.Remove(additive);
        currentValue = Mathf.Clamp(currentValue, 0, GetValue());
    }

    public float GetCurrentValue()
    {
        return currentValue;
    }

    public void ChangeCurrentValue(float amount)
    {
        float temp = currentValue + amount;
        currentValue = Mathf.Clamp(temp, 0, GetValue());
    }
}
