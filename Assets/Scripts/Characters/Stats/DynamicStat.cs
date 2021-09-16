using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[System.Serializable]
public class DynamicStat : Stat
{
    public int currentValue;

    public DynamicStat(int baseValue) : base(baseValue)
    {
        this.baseValue = baseValue;
        this.currentValue = baseValue;
    }

    public override void AddModifier(int modifier)
    {
        if(modifier != 0)
            modifiers.Add(modifier);     
        
        currentValue = Mathf.Clamp(currentValue, 0, GetValue());
    }

    public override void RemoveModifier(int modifier)
    {
        if(modifier != 0)
            modifiers.Remove(modifier);

        currentValue = Mathf.Clamp(currentValue, 0, GetValue());
    }

    public int GetCurrentValue()
    {
        return currentValue;
    }

    public void ChangeCurrentValue(int amount)
    {
        int temp = currentValue + amount;
        currentValue = Mathf.Clamp(temp, 0, GetValue());
    }
}
