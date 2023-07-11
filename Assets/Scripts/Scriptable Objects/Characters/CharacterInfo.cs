using System.Collections;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;

public enum StatType
{
    Attack,
    MAttack,
    Defense,
    MDefense,
    Healing,
    Agility,
    HP,
    MP
}

public enum SecondaryStatType
{
    CritRate,
    CritPower,
    EvadeRate,
    BlockRate,
    BlockPower
}

public enum ElementalResistance
{
    Neutral,
    Weak,
    Resist,
    Null
}

public class CharacterInfo : ScriptableObject
{
    [Header("HP/MP")]
    [SerializeField] private int baseHP;
    [SerializeField] private float hpGrowth;
    [SerializeField] private int baseMP;
    [SerializeField] private float mpGrowth;

    [Header("Primary Stats")]
    [SerializeField] private int baseAttack;
    [SerializeField] private float attackGrowth;
    [SerializeField] private int baseDefense;
    [SerializeField] private float defenseGrowth;
    [SerializeField] private int baseMAttack;
    [SerializeField] private float mAttackGrowth;
    [SerializeField] private int baseMDefense;
    [SerializeField] private float mDefenseGrowth;
    [SerializeField] private int baseHealing;
    [SerializeField] private float healingGrowth;
    [SerializeField] private int baseAgility;
    [SerializeField] private float agilityGrowth;

    [Header("Secondary Stats")]
    [SerializeField] private float baseCritRate;
    [SerializeField] private float baseCritPower;
    [SerializeField] private float baseEvadeRate;
    [SerializeField] private float baseBlockRate;
    [SerializeField] private float baseBlockPower;

    [Header("Elemental Resistances")]
    [SerializeField] private ElementalResistance fireResistance;
    [SerializeField] private ElementalResistance iceResistance;
    [SerializeField] private ElementalResistance electricResistance;
    [SerializeField] private ElementalResistance darkResistance;

    [field: Header("Images and Sprites")]
    [field: SerializeField] public Sprite TurnIcon { get; private set; }
    [field: SerializeField] public string CharacterName { get; private set; } = "";
    [field: SerializeField] public int Level { get; private set; } = 1;
    //the character's starting stats
    public Dictionary<StatType, int> BaseStats { get; private set; }
    //bonus stats per level
    public Dictionary<StatType, float> StatGrowth { get; private set; }
    public Dictionary<StatType, List<int>> StatModifiers { get; private set; }
    public Dictionary<SecondaryStatType, float> SecondaryStats { get; private set; }
    public Dictionary<ElementalProperty, ElementalResistance> Resistances { get; private set; }
    public Dictionary<ElementalProperty, List<int>> ResistanceModifiers { get; private set; }
    [field: SerializeField] public List<Trait> Traits { get; private set; }
    public List<ActionModifier> ActionModifiers { get; private set; }
    public List<TriggerableBattleEffect> TriggerableBattleEffects { get; private set; }

    protected virtual void OnEnable()
    {
        //base stats
        BaseStats = new Dictionary<StatType, int>();
        BaseStats.Add(StatType.Attack, baseAttack);
        BaseStats.Add(StatType.Defense, baseDefense);
        BaseStats.Add(StatType.MAttack, baseMAttack);
        BaseStats.Add(StatType.MDefense, baseMDefense);
        BaseStats.Add(StatType.Agility, baseAgility);
        BaseStats.Add(StatType.Healing, baseHealing);
        BaseStats.Add(StatType.HP, baseHP);
        BaseStats.Add(StatType.MP, baseMP);

        //secondary stats (% based, not obtained via leveling)
        SecondaryStats = new Dictionary<SecondaryStatType, float>();
        SecondaryStats.Add(SecondaryStatType.CritRate, baseCritRate);
        SecondaryStats.Add(SecondaryStatType.CritPower, baseCritPower);
        SecondaryStats.Add(SecondaryStatType.EvadeRate, baseEvadeRate);
        SecondaryStats.Add(SecondaryStatType.BlockRate, baseBlockRate);
        SecondaryStats.Add(SecondaryStatType.BlockPower, baseBlockPower);

        //stat growth dict
        StatGrowth = new Dictionary<StatType, float>();
        StatGrowth.Add(StatType.Attack, attackGrowth);
        StatGrowth.Add(StatType.Defense, defenseGrowth);
        StatGrowth.Add(StatType.MAttack, mAttackGrowth);
        StatGrowth.Add(StatType.MDefense, mDefenseGrowth);
        StatGrowth.Add(StatType.Agility, agilityGrowth);
        StatGrowth.Add(StatType.Healing, healingGrowth);
        StatGrowth.Add(StatType.HP, hpGrowth);
        StatGrowth.Add(StatType.MP, mpGrowth);

        //stat modifier dict
        StatModifiers = new Dictionary<StatType, List<int>>();
        foreach (StatType statType in System.Enum.GetValues(typeof(StatType)))
        {
            StatModifiers.Add(statType, new List<int>());
        }

        Resistances = new Dictionary<ElementalProperty, ElementalResistance>();
        Resistances.Add(ElementalProperty.None, ElementalResistance.Neutral);
        Resistances.Add(ElementalProperty.Fire, fireResistance);
        Resistances.Add(ElementalProperty.Ice, iceResistance);
        Resistances.Add(ElementalProperty.Electric, electricResistance);
        Resistances.Add(ElementalProperty.Dark, darkResistance);

        ResistanceModifiers = new Dictionary<ElementalProperty, List<int>>();
        ResistanceModifiers.Add(ElementalProperty.None, new List<int>());
        ResistanceModifiers.Add(ElementalProperty.Fire, new List<int>());
        ResistanceModifiers.Add(ElementalProperty.Ice, new List<int>());
        ResistanceModifiers.Add(ElementalProperty.Electric, new List<int>());
        ResistanceModifiers.Add(ElementalProperty.Dark, new List<int>());

        TriggerableBattleEffects = new List<TriggerableBattleEffect>();
        ActionModifiers = new List<ActionModifier>();
    }

    public int GetStat(StatType statType)
    {
        int stat = 0;
        stat += Mathf.FloorToInt(BaseStats[statType] + (Level * StatGrowth[statType]) + StatModifiers[statType].Sum());
        return stat;
    }
}
