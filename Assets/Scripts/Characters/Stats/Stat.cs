using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public enum StatType
{
    Health,
    Mana,
    Attack,
    Defense,
    Special,
    Agility,
    Skill,
    MoveRange
}

[System.Serializable]
public class Stat
{
    [SerializeField] protected int baseValue;
    public StatType statType;
    [SerializeField] protected List<int> modifiers = new List<int>();

    public Stat(StatType statType, int baseValue)
    {
        this.statType = statType;
        this.baseValue = baseValue;
    }

    public int GetValue()
    {
        int finalValue = baseValue;
        modifiers.ForEach(x => finalValue += x);
        return finalValue;
    }

    public void ChangeValue(int amount)
    {
        baseValue = baseValue + amount;
    }

    public virtual void AddModifier(int modifier)
    {
        if(modifier != 0)
            modifiers.Add(modifier);
        
    }

    public virtual void RemoveModifier(int modifier)
    {
        if(modifier != 0)
            modifiers.Remove(modifier);
    }

    public virtual void RemoveAllModifiers()
    {
        modifiers.Clear();
    }
}
