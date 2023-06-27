using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[System.Serializable]
public class DynamicStat : Stat
{
    [SerializeField] private int currentValue;
    public int CurrentValue { get; private set; }

    public DynamicStat(int _baseValue, int _currentValue) : base(_baseValue)
    {
        BaseValue = _baseValue;
        CurrentValue = _currentValue;
    }

    public override void AddModifier(float modifier)
    {
        modifiers.Add(modifier);
        int amountToAdd = Mathf.FloorToInt(CurrentValue * modifier);
        CurrentValue =  Mathf.Clamp(CurrentValue + amountToAdd, 1, GetValue());
    }

    public override void RemoveModifier(float modifier)
    {
        modifiers.Remove(modifier);
        int amountToRemove = Mathf.FloorToInt(CurrentValue * modifier);
        CurrentValue = Mathf.Clamp(CurrentValue - amountToRemove, 1, GetValue());
    }

    public void ChangeCurrentValue(int amount)
    {
        CurrentValue = Mathf.Clamp(CurrentValue + amount, 0, GetValue());
    }
}
