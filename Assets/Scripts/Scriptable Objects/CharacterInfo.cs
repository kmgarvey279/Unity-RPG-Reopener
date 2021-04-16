using System.Collections;
using System.Collections.Generic;
using UnityEngine;

// public enum ElementalProperty
// {
//     Fire,
//     Ice,
//     Electric,
//     Dark
// }

// public enum AttackProperty
// {
//     Strike,
//     Slash,
//     Pierce
// }

public class CharacterInfo : ScriptableObject
{
    public string name;
    public int level;

    [Header("HP/MP")]
    public DynamicStat health;
    public DynamicStat mana;

    [Header("Stats")]
    public Stat attack;
    public Stat defense;
    public Stat special;

    public float moveSpeed;
    public Dictionary<StatType, Stat> statDict = new Dictionary<StatType, Stat>();

    [Header("Resistances")]
    public float fire;
    public float ice;
    public float electric;
    public float dark;
    // public Dictionary<AttackProperty, float> resistDict;

    protected virtual void OnEnable()
    {
        //add each stat to dictionary
        statDict.Add(StatType.Health, health);
        statDict.Add(StatType.Mana, mana);
        statDict.Add(StatType.Attack, attack);
        statDict.Add(StatType.Defense, defense);
        statDict.Add(StatType.Special, special);
        //set type of each stat
        foreach(KeyValuePair<StatType,Stat> entry in statDict)
        {
            entry.Value.statType = entry.Key; 
        }
    }

    protected void OnDisable()
    {
        foreach(KeyValuePair<StatType,Stat> entry in statDict)
        {
            entry.Value.RemoveAllModifiers();
        }   
    }
}
