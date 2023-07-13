using System.Collections;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;

//standard int stats, increase with level
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

//misc. float stats, not effected by level
public enum SecondaryStatType
{
    CritRate,
    CritPower,
    EvadeRate,
    BlockPower,
    ChainStartBonus,
    ChainContributionBonus
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
    [SerializeField] protected int baseHP;
    public Stat HP;
    [SerializeField] protected int baseMP;
    public Stat MP;

    [Header("Primary Stats")]
    [SerializeField] protected int baseAttack;
    [SerializeField] protected int baseDefense;
    [SerializeField] protected int baseMAttack;
    [SerializeField] protected int baseMDefense;
    [SerializeField] protected int baseHealing;
    [SerializeField] protected int baseAgility;
    public Dictionary<StatType, Stat> Stats { get; protected set; } 

    //[SerializeField] private float hpGrowth;
    //[SerializeField] private float mpGrowth;
    //[SerializeField] private float attackGrowth;
    //[SerializeField] private float defenseGrowth;
    //[SerializeField] private float mAttackGrowth;
    //[SerializeField] private float mDefenseGrowth;
    //[SerializeField] private float healingGrowth;
    //[SerializeField] private float agilityGrowth;
    //public Dictionary<StatType, float> StatGrowth { get; private set; }
    public Dictionary<SecondaryStatType, SecondaryStat> SecondaryStats { get; protected set; } 

    [Header("Elemental Resistances")]
    [SerializeField] protected ElementalResistance fireResistance;
    [SerializeField] protected ElementalResistance iceResistance;
    [SerializeField] protected ElementalResistance electricResistance;
    [SerializeField] protected ElementalResistance darkResistance;
    public Dictionary<ElementalProperty, ElementalResistance> Resistances { get; protected set; }
    public Dictionary<ElementalProperty, List<float>> ResistanceModifiers { get; protected set; }

    [field: Header("Images and Sprites")]
    [field: SerializeField] public Sprite TurnIcon { get; protected set; }
    [field: SerializeField] public string CharacterName { get; protected set; } = "";
    [field: SerializeField] public int Level { get; protected set; } = 1;
    [field: SerializeField] public List<Trait> Traits { get; protected set; }
    public List<ActionModifier> ActionModifiers { get; protected set; }
    public List<TriggerableBattleEffect> TriggerableBattleEffects { get; protected set; }

    protected virtual void OnEnable()
    {
        //HP/MP
        HP = new Stat(baseHP);
        MP = new Stat(baseMP);

        //base stats
        Stats = new Dictionary<StatType, Stat>();
        Stats.Add(StatType.Attack, new Stat(baseAttack));
        Stats.Add(StatType.Defense, new Stat(baseDefense));
        Stats.Add(StatType.MAttack, new Stat(baseMAttack));
        Stats.Add(StatType.MDefense, new Stat(baseMDefense));
        Stats.Add(StatType.Agility, new Stat(baseAgility));
        Stats.Add(StatType.Healing, new Stat(baseHealing));

        //secondary stats (% based, not obtained via leveling)
        SecondaryStats = new Dictionary<SecondaryStatType, SecondaryStat>();
        foreach (SecondaryStatType secondaryStatType in System.Enum.GetValues(typeof(SecondaryStatType)))
        {
            SecondaryStats.Add(secondaryStatType, new SecondaryStat(0));
        }

            //stat growth dict
            //StatGrowth = new Dictionary<StatType, float>();
            //StatGrowth.Add(StatType.Attack, attackGrowth);
            //StatGrowth.Add(StatType.Defense, defenseGrowth);
            //StatGrowth.Add(StatType.MAttack, mAttackGrowth);
            //StatGrowth.Add(StatType.MDefense, mDefenseGrowth);
            //StatGrowth.Add(StatType.Agility, agilityGrowth);
            //StatGrowth.Add(StatType.Healing, healingGrowth);
            //StatGrowth.Add(StatType.HP, hpGrowth);
            //StatGrowth.Add(StatType.MP, mpGrowth);

            Resistances = new Dictionary<ElementalProperty, ElementalResistance>();
        Resistances.Add(ElementalProperty.None, ElementalResistance.Neutral);
        Resistances.Add(ElementalProperty.Fire, fireResistance);
        Resistances.Add(ElementalProperty.Ice, iceResistance);
        Resistances.Add(ElementalProperty.Electric, electricResistance);
        Resistances.Add(ElementalProperty.Dark, darkResistance);

        ResistanceModifiers = new Dictionary<ElementalProperty, List<float>>();
        ResistanceModifiers.Add(ElementalProperty.None, new List<float>());
        ResistanceModifiers.Add(ElementalProperty.Fire, new List<float>());
        ResistanceModifiers.Add(ElementalProperty.Ice, new List<float>());
        ResistanceModifiers.Add(ElementalProperty.Electric, new List<float>());
        ResistanceModifiers.Add(ElementalProperty.Dark, new List<float>());

        TriggerableBattleEffects = new List<TriggerableBattleEffect>();
        ActionModifiers = new List<ActionModifier>();
    }
}
