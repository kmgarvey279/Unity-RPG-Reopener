using System.Collections;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;

//public enum ElementalResistance
//{
//    Neutral,
//    Weak,
//    Resist,
//    Null
//}

[System.Serializable]
public class CharacterInfo : ScriptableObject
{
    public ClampInt CurrentHP { get; protected set; }
    public ClampInt CurrentMP { get; protected set; }

    [Header("Primary Stats")]
    [SerializeField] protected int baseHP;
    [SerializeField] protected int currentHP;
    [SerializeField] protected int baseMP;
    [SerializeField] protected int currentMP;

    [SerializeField] protected int baseAttack;
    [SerializeField] protected int baseDefense;
    [SerializeField] protected int baseMAttack;
    [SerializeField] protected int baseMDefense;
    [SerializeField] protected int baseHealing;
    [SerializeField] protected int baseAgility;
    [SerializeField] protected int baseCritRate;
    [SerializeField] protected int baseEvadeRate;

    public Dictionary<IntStatType, IntStat> IntStats { get; protected set; } 

    //[SerializeField] private float hpGrowth;
    //[SerializeField] private float mpGrowth;
    //[SerializeField] private float attackGrowth;
    //[SerializeField] private float defenseGrowth;
    //[SerializeField] private float mAttackGrowth;
    //[SerializeField] private float mDefenseGrowth;
    //[SerializeField] private float healingGrowth;
    //[SerializeField] private float agilityGrowth;
    //public Dictionary<StatType, float> StatGrowth { get; private set; }
    //public Dictionary<FloatStatType, FloatStat> FloatStats { get; protected set; } 

    [field: Header("Images and Sprites")]
    [field: SerializeField] public Sprite TurnIcon { get; protected set; }
    [field: SerializeField] public string CharacterName { get; protected set; } = "";
    [field: SerializeField] public int Level { get; protected set; } = 1;
    [field: SerializeField] public List<Trait> Traits { get; protected set; }

    protected virtual void OnEnable()
    {
        IntStats = new Dictionary<IntStatType, IntStat>();
        foreach (IntStatType intStatType in System.Enum.GetValues(typeof(IntStatType)))
        {
            IntStats.Add(intStatType, new IntStat(0));
        }

        //IntStats[IntStatType.MaxHP].UpdateBaseValue(baseHP);
        //CurrentHP = new ClampInt(currentHP, 1, baseHP);
        //IntStats[IntStatType.MaxMP].UpdateBaseValue(baseMP);
        //CurrentMP = new ClampInt(currentMP, 0, baseMP);

        //IntStats[IntStatType.Attack].UpdateBaseValue(baseAttack);
        //IntStats[IntStatType.Defense].UpdateBaseValue(baseDefense);
        //IntStats[IntStatType.MAttack].UpdateBaseValue(baseMAttack);
        //IntStats[IntStatType.MDefense].UpdateBaseValue(baseMDefense);
        //IntStats[IntStatType.Agility].UpdateBaseValue(baseAgility);
        //IntStats[IntStatType.Healing].UpdateBaseValue(baseHealing);
        //IntStats[IntStatType.CritRate].UpdateBaseValue(baseCritRate);
        //IntStats[IntStatType.EvadeRate].UpdateBaseValue(baseEvadeRate);


        //FloatStats = new Dictionary<FloatStatType, FloatStat>();
        //foreach (FloatStatType floatStatType in System.Enum.GetValues(typeof(FloatStatType)))
        //{
        //    FloatStats.Add(floatStatType, new FloatStat(0));
        //}

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

        //Resistances = new Dictionary<ElementalProperty, ElementalResistance>();
        //Resistances.Add(ElementalProperty.Physical, physicalResistance);
        //Resistances.Add(ElementalProperty.Fire, fireResistance);
        //Resistances.Add(ElementalProperty.Ice, iceResistance);
        //Resistances.Add(ElementalProperty.Electric, electricResistance);
        //Resistances.Add(ElementalProperty.Dark, darkResistance);
        //Resistances.Add(ElementalProperty.None, ElementalResistance.Neutral);

        //ResistanceModifiers = new Dictionary<ElementalProperty, List<float>>();
        //ResistanceModifiers.Add(ElementalProperty.Physical, new List<float>());
        //ResistanceModifiers.Add(ElementalProperty.Fire, new List<float>());
        //ResistanceModifiers.Add(ElementalProperty.Ice, new List<float>());
        //ResistanceModifiers.Add(ElementalProperty.Electric, new List<float>());
        //ResistanceModifiers.Add(ElementalProperty.Dark, new List<float>());
        //ResistanceModifiers.Add(ElementalProperty.None, new List<float>());

        //foreach (Trait trait in Traits)
        //{
        //    ApplyTrait(trait);
        //}
    }

    public void ApplyIntStatModifier(IntStatType statType, float modifierValue, ValueModifierType valueModifierType)
    {
        //IntStats[statType].AddModifier(modifierValue, valueModifierType);
        //if (statType == IntStatType.MaxHP)
        //{
        //    CurrentHP = new ClampInt(CurrentHP.CurrentValue, 1, IntStats[IntStatType.MaxHP].CurrentValue);
        //}
        //else if (statType == IntStatType.MaxMP)
        //{
        //    CurrentMP = new ClampInt(CurrentMP.CurrentValue, 0, IntStats[IntStatType.MaxMP].CurrentValue);
        //}
    }

    public void RemoveIntStatModifier(IntStatType statType, float modifierValue, ValueModifierType modifierType)
    {
        //IntStats[statType].RemoveModifier(modifierValue, modifierType);
        //if (statType == IntStatType.MaxHP)
        //{
        //    CurrentHP = new ClampInt(CurrentHP.CurrentValue, 1, IntStats[IntStatType.MaxHP].CurrentValue);
        //}
        //else if (statType == IntStatType.MaxMP)
        //{
        //    CurrentMP = new ClampInt(CurrentMP.CurrentValue, 0, IntStats[IntStatType.MaxMP].CurrentValue);
        //}
    }

    //protected void ApplyTrait(Trait trait)
    //{
    //    foreach (IntStatModifier statModifier in trait.IntStatModifiers)
    //    {
    //        ApplyIntStatModifier(statModifier.IntStatType, statModifier.Modifier, statModifier.ModifierType);
    //    }
    //    foreach (FloatStatModifier statModifier in trait.FloatStatModifiers)
    //    {
    //        FloatStats[statModifier.FloatStatType].AddModifier(statModifier.Modifier);
    //    }
    //}

    //protected void RemoveTrait(Trait trait)
    //{
    //    foreach (IntStatModifier statModifier in trait.IntStatModifiers)
    //    {
    //        RemoveIntStatModifier(statModifier.IntStatType, statModifier.Modifier, statModifier.ModifierType);
    //    }
    //    foreach (FloatStatModifier statModifier in trait.FloatStatModifiers)
    //    {
    //        FloatStats[statModifier.FloatStatType].RemoveModifier(statModifier.Modifier);
    //    }
    //}
}
