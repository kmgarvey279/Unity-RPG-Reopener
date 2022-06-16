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
    Agility
}

public enum ResistanceType
{
    Low,
    Neutral,
    High, 
    Null
}

public class ElementalResistance
{
    public ResistanceType resistanceType;
    public int resistance;

    public float GetResistMultiplier()
    {
        float multiplier = 1f;
        switch(resistanceType)
        {
            case ResistanceType.Low:
                multiplier = 1.4f;
                break;
            case ResistanceType.High:
                multiplier = 0.6f;
                break;
            default:
                break;
        }
        return 1f - ((float)resistance * 0.01f) * multiplier;
    }
    public ElementalResistance(ResistanceType resistanceType)
    {
        this.resistanceType = resistanceType;
        resistance = 0;
    }
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
    [SerializeField] private int attackStat;
    //determine defense against physical/gun attacks
    [SerializeField] private int defenseStat;
    //determine offensive and defensive magic power + mp regen
    [SerializeField] private int magicAttackStat;
    [SerializeField] private int magicDefenseStat;
    //determine accuracy and crit rate/damage
    [SerializeField] private int skillStat;
    //determine action speed/cost and evasion 
    [SerializeField] private int agilityStat;
    public Dictionary<StatType, Stat> statDict;

    [Header("Elemental Resistances")]
    [SerializeField] private ResistanceType fireResist = ResistanceType.Neutral;
    [SerializeField] private ResistanceType iceResist = ResistanceType.Neutral;
    [SerializeField] private ResistanceType electricResist = ResistanceType.Neutral;
    [SerializeField] private ResistanceType darkResist = ResistanceType.Neutral;
    [SerializeField] private ResistanceType lightResist = ResistanceType.Neutral;
    public Dictionary<ElementalProperty, ElementalResistance> resistDict;

    [Header("Traits")]
    public List<Trait> traits;

    [Header("Actions")]
    public Action attack;
    public CounterAction counterAction;

    [Header("ActionEventModifiers")]
    public List<ActionEventModifier> actionEventModifiers = new List<ActionEventModifier>();


    protected virtual void OnEnable()
    {
        hp = new DynamicStat(baseHP, baseHP);
        mp = new DynamicStat(baseMP, baseMP);
        //add each stat to dictionary
        statDict = new Dictionary<StatType, Stat>();
        statDict.Add(StatType.Attack, new Stat(attackStat));
        statDict.Add(StatType.Defense, new Stat(defenseStat));
        statDict.Add(StatType.MagicAttack, new Stat(magicAttackStat));
        statDict.Add(StatType.MagicDefense, new Stat(magicDefenseStat));
        statDict.Add(StatType.Skill, new Stat(skillStat));
        statDict.Add(StatType.Agility, new Stat(agilityStat));

        resistDict = new Dictionary<ElementalProperty, ElementalResistance>();
        resistDict.Add(ElementalProperty.None, new ElementalResistance(ResistanceType.Neutral));
        resistDict.Add(ElementalProperty.Fire, new ElementalResistance(fireResist));
        resistDict.Add(ElementalProperty.Ice, new ElementalResistance(iceResist));
        resistDict.Add(ElementalProperty.Electric, new ElementalResistance(electricResist));
        resistDict.Add(ElementalProperty.Dark, new ElementalResistance(darkResist));
        resistDict.Add(ElementalProperty.Light, new ElementalResistance(lightResist));
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
