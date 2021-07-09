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
    [SerializeField] private int attack;
    [SerializeField] private int defense;
    [SerializeField] private int special;
    [SerializeField] private int agility;
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
    public Dictionary<AttackProperty, int> resistDict;

    [Header("Skills")]
    public Action meleeAttack;
    public Action rangedAttack;
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
