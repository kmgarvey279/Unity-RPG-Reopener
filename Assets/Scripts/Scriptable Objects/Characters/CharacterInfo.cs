using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CharacterInfo : ScriptableObject
{
    [Header("Basic info")]
    public string characterName;
    public int level;

    [Header("HP/MP")]
    [SerializeField] private int baseHP;
    [HideInInspector] public DynamicStat hp;
    [SerializeField] private int baseMP;
    [HideInInspector] public DynamicStat mp;

    [Header("offensive and defensive power")]
    [SerializeField] private int meleePower;
    [SerializeField] private int rangedPower;
    [SerializeField] private int magicPower;
    [SerializeField] private int meleeResistance;
    [SerializeField] private int rangedResistance;
    [SerializeField] private int magicResistance;

    [Header("Stats")]
    //determine power of physical/gun attacks
    [SerializeField] private int attack;
    //determine accuracy and crit rate/damage
    [SerializeField] private int skill;
    //determine defense against physical/gun attacks
    [SerializeField] private int defense;
    //determine offensive and defensive magic power + mp regen
    [SerializeField] private int special;
    //determine action speed/cost and evasion 
    [SerializeField] private int agility;
    [SerializeField] private int moveRange;
    public Dictionary<StatType, Stat> statDict;

    [Header("Elemental Resistances")]
    [SerializeField] private int fireResist;
    [SerializeField] private int iceResist;
    [SerializeField] private int electricResist;
    [SerializeField] private int voidResist;
    public Dictionary<ElementalProperty, int> resistDict;

    [Header("Skills")]
    public Action meleeAttack;
    public Action rangedAttack;
    public List<Action> skills = new List<Action>();


    protected virtual void OnEnable()
    {
        hp = new DynamicStat(StatType.HP, baseHP);
        mp = new DynamicStat(StatType.MP, baseMP);
        //add each stat to dictionary
        statDict = new Dictionary<StatType, Stat>();
        statDict.Add(StatType.Attack, new Stat(StatType.Attack, attack));
        statDict.Add(StatType.Defense, new Stat(StatType.Defense, defense));
        statDict.Add(StatType.Special, new Stat(StatType.Special, special));
        statDict.Add(StatType.Agility, new Stat(StatType.Agility, agility));
        statDict.Add(StatType.Skill, new Stat(StatType.Skill, skill));
        statDict.Add(StatType.MoveRange, new Stat(StatType.MoveRange, moveRange));
        //add resistances to dictionary
        resistDict = new Dictionary<ElementalProperty, int>();
        resistDict.Add(ElementalProperty.Fire, fireResist);
        resistDict.Add(ElementalProperty.Ice, iceResist);
        resistDict.Add(ElementalProperty.Electric, electricResist);
        resistDict.Add(ElementalProperty.Void, voidResist);
    }

    public virtual void OnDisable()
    {
        foreach(KeyValuePair<StatType,Stat> entry in statDict)
        {
            entry.Value.RemoveAllModifiers();
        }  
        statDict.Clear(); 
    }
}
