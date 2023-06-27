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
    Agility,
    Crit,
    Evade,
    HP,
    MP
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

    [Header("Stats")]
    [SerializeField] private int baseAttack;
    [SerializeField] private float attackGrowth;
    [SerializeField] private int baseDefense;
    [SerializeField] private float defenseGrowth;
    [SerializeField] private int baseMAttack;
    [SerializeField] private float mAttackGrowth;
    [SerializeField] private int baseMDefense;
    [SerializeField] private float mDefenseGrowth;
    [SerializeField] private int baseAgility;
    [SerializeField] private float agilityGrowth;
    [SerializeField] private int baseCrit;
    [SerializeField] private int baseEvade;

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
    //bonus stats via equipment and traits
    public Dictionary<StatType, List<int>> StatModifiers { get; private set; }
    public Dictionary<ElementalProperty, ElementalResistance> Resistances { get; private set; }
    public Dictionary<ElementalProperty, List<int>> ResistanceModifiers { get; private set; }
    [field: SerializeField] public List<Trait> Traits { get; private set; }
    public List<ActionModifier> ActionModifiers { get; private set; }
    public List<TriggerableBattleEffect> TriggerableBattleEffects { get; private set; }

    protected virtual void OnEnable()
    {
        //base stat dict
        BaseStats = new Dictionary<StatType, int>();
        BaseStats.Add(StatType.Attack, baseAttack);
        BaseStats.Add(StatType.Defense, baseDefense);
        BaseStats.Add(StatType.MAttack, baseMAttack);
        BaseStats.Add(StatType.MDefense, baseMDefense);
        BaseStats.Add(StatType.Agility, baseAgility);
        BaseStats.Add(StatType.Crit, baseCrit);
        BaseStats.Add(StatType.Evade, baseEvade);
        BaseStats.Add(StatType.HP, baseHP);
        BaseStats.Add(StatType.MP, baseMP);

        //stat growth dict
        StatGrowth = new Dictionary<StatType, float>();
        StatGrowth.Add(StatType.Attack, attackGrowth);
        StatGrowth.Add(StatType.Defense, defenseGrowth);
        StatGrowth.Add(StatType.MAttack, mAttackGrowth);
        StatGrowth.Add(StatType.MDefense, mDefenseGrowth);
        StatGrowth.Add(StatType.Agility, agilityGrowth);
        StatGrowth.Add(StatType.Crit, 0);
        StatGrowth.Add(StatType.Evade, 0);
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
        stat += BaseStats[statType] + Mathf.FloorToInt((Level * StatGrowth[statType])) + StatModifiers[statType].Sum();
        return stat;
    }
}
