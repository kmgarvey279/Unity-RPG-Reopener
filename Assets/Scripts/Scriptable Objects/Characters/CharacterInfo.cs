using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public enum StatType
{
    //inherent stats
    HP,
    MP,
    Attack,
    Defense,
    MagicAttack,
    MagicDefense,
    Skill,
    Agility,
    //gear stats
    EquipmentAttack,
    EquipmentMagicAttack,
    EquipmentDefense,
    EquipmentMagicDefense
}

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

    [Header("Stats")]
    //determine power of physical/gun attacks
    [SerializeField] private int attack;
    //determine defense against physical/gun attacks
    [SerializeField] private int defense;
    //determine offensive and defensive magic power + mp regen
    [SerializeField] private int magicAttack;
    [SerializeField] private int magicDefense;
    //determine accuracy and crit rate/damage
    [SerializeField] private int skill;
    //determine action speed/cost and evasion 
    [SerializeField] private int agility;
    public Dictionary<StatType, Stat> statDict;

    [Header("Elemental Resistances")]
    [SerializeField] private int fireResist;
    [SerializeField] private int iceResist;
    [SerializeField] private int electricResist;
    [SerializeField] private int darkResist;
    public Dictionary<ElementalProperty, Stat> resistDict;

    [Header("Traits")]
    public List<Trait> traits;

    protected virtual void OnEnable()
    {
        hp = new DynamicStat(baseHP, baseHP);
        mp = new DynamicStat(baseMP, baseMP);
        //add each stat to dictionary
        statDict = new Dictionary<StatType, Stat>();
        statDict.Add(StatType.Attack, new Stat(attack));
        statDict.Add(StatType.Defense, new Stat(defense));
        statDict.Add(StatType.MagicAttack, new Stat(magicAttack));
        statDict.Add(StatType.MagicDefense, new Stat(magicDefense));
        statDict.Add(StatType.Skill, new Stat(skill));
        statDict.Add(StatType.Agility, new Stat(agility));
        //add resistances to dictionary
        resistDict = new Dictionary<ElementalProperty, Stat>();
        resistDict.Add(ElementalProperty.Fire, new Stat(fireResist));
        resistDict.Add(ElementalProperty.Ice, new Stat(iceResist));
        resistDict.Add(ElementalProperty.Electric, new Stat(electricResist));
        resistDict.Add(ElementalProperty.Dark, new Stat(darkResist));
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
