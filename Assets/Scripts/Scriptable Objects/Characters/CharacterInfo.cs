using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public enum AttackProperty
{
    None,
    Strike, 
    Slash,
    Pierce,
    Fire,
    Ice,
    Electric,
    Dark
}

public class CharacterInfo : ScriptableObject
{
    [Header("Basic info")]
    public string characterName;
    public int level;

    [Header("HP/MP")]
    [SerializeField] private int baseHealth;
    [HideInInspector] public DynamicStat health;
    [SerializeField] private int baseMana;
    [HideInInspector] public DynamicStat mana;

    [Header("Stats")]
    //determine power of physical/gun attacks
    [SerializeField] private int attack;
    //determine defense against physical/gun attacks
    [SerializeField] private int defense;
    //determine offensive and defensive magic power + mp regen
    [SerializeField] private int special;
    //determine action speed/cost and evasion 
    [SerializeField] private int agility;
    //determine accuracy and crit rate/damage
    [SerializeField] private int skill;
    [SerializeField] private int moveRange;
    public Dictionary<StatType, Stat> statDict;

    [Header("Physical Resistances")]
    [SerializeField] private int strike;
    [SerializeField] private int slash;
    [SerializeField] private int pierce;
    [Header("Elemental Resistances")]
    [SerializeField] private int fire;
    [SerializeField] private int ice;
    [SerializeField] private int electric;
    [SerializeField] private int dark;
    // [SerializeField] private int light;
    // [SerializeField] private int water;
    // [SerializeField] private int wind;
    // [SerializeField] private int earth;
    public Dictionary<AttackProperty, int> resistDict;

    [Header("Skills")]
    public Action attack1;
    public Action attack2;
    public List<Action> skills = new List<Action>();


    protected virtual void OnEnable()
    {
        health = new DynamicStat(StatType.Health, baseHealth);
        mana = new DynamicStat(StatType.Mana, baseMana);
        //add each stat to dictionary
        statDict = new Dictionary<StatType, Stat>();
        statDict.Add(StatType.Attack, new Stat(StatType.Attack, attack));
        statDict.Add(StatType.Defense, new Stat(StatType.Defense, defense));
        statDict.Add(StatType.Special, new Stat(StatType.Special, special));
        statDict.Add(StatType.Agility, new Stat(StatType.Agility, agility));
        statDict.Add(StatType.Skill, new Stat(StatType.Skill, skill));
        statDict.Add(StatType.MoveRange, new Stat(StatType.MoveRange, moveRange));
        //add resistances to dictionary
        resistDict = new Dictionary<AttackProperty, int>();
        resistDict.Add(AttackProperty.Strike, strike);
        resistDict.Add(AttackProperty.Slash, slash);
        resistDict.Add(AttackProperty.Pierce, pierce);
        resistDict.Add(AttackProperty.Fire, fire);
        resistDict.Add(AttackProperty.Ice, ice);
        resistDict.Add(AttackProperty.Electric, electric);
        resistDict.Add(AttackProperty.Dark, dark);
    }

    protected void OnDisable()
    {
        foreach(KeyValuePair<StatType,Stat> entry in statDict)
        {
            entry.Value.RemoveAllModifiers();
        }  
        statDict.Clear(); 
    }
}
