using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[System.Serializable]
public class DynamicStat : Stat
{
    public float currentValue;

    private void OnEnable()
    {
        currentValue = baseValue;
    }

    public override void AddModifier(float modifier)
    {
        if(modifier != 0)
            modifiers.Add(modifier);     
        
        currentValue = Mathf.Clamp(currentValue, 0, GetValue());
    }

    public override void RemoveModifier(float modifier)
    {
        if(modifier != 0)
            modifiers.Remove(modifier);

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
