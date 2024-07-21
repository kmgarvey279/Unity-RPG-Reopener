using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public enum ElementalResistance
{
    Neutral,
    Weak,
    Resist,
    Null
}

[System.Serializable]
public class WeightedAction
{
    [System.Serializable]
    private enum WeightType
    {
        VeryLow,
        Low,
        Mid,
        High,
        VeryHigh
    }
    private Dictionary<WeightType, int> weightDict = new Dictionary<WeightType, int>()
    {
        {WeightType.VeryLow, 10},
        {WeightType.Low, 20},
        {WeightType.Mid, 30},
        {WeightType.High, 40},
        {WeightType.VeryHigh, 50}
    };

    [SerializeField] private WeightType weightType;

    [field: SerializeField] public Action Action { private set; get; }
    [field: SerializeField] public ActionType ActionType { private set; get; }

    public int BaseWeight()
    {
        return weightDict[weightType];
    }

    public void SetAction(Action action)
    {
        Action = action;
    }
}

[CreateAssetMenu(fileName = "New Enemy Info", menuName = "Enemy Info")]
public class EnemyInfo : ScriptableObject
{
    public string EnemyID { private set; get; }
    public string enemyIDCopy;

    [field: Header("Primary Info")]
    [field: SerializeField] public string CharacterName { get; private set; } = "";
    [field: SerializeField] public GameObject Prefab { get; private set; }
    [field: SerializeField] public Sprite TurnIcon { get; private set; }
    [field: SerializeField] public bool IsBoss { get; private set; }
    [field: Header("Stats")]
    [SerializeField] private float hpGrowth;
    [SerializeField] private float mpGrowth;
    [SerializeField] private float attackGrowth;
    [SerializeField] private float defenseGrowth;
    [SerializeField] private float mAttackGrowth;
    [SerializeField] private float mDefenseGrowth;
    [SerializeField] private float agiltyGrowth;
    [SerializeField] private float healingGrowth;
    [field: SerializeField] public int Level { get; protected set; } = 1;
    public Dictionary<IntStatType, float> StatGrowth { get; protected set; }
    [field: Header("Guard")]
    [field: SerializeField] public int Guard { get; protected set; }
    [field: Header("Traits")]
    [field: SerializeField] public List<Trait> Traits { get; protected set; }
    [field: Header("Skills")]
    [field: SerializeField] public List<WeightedAction> WeightedActions { get; protected set; } = new List<WeightedAction>();
    [field: Header("Vulnerabilities")]
    [field: SerializeField] public List<ElementalProperty> Vulnerabilities { get; protected set; } = new List<ElementalProperty>();

    private void Awake()
    {
        if (EnemyID == "")
        {
            EnemyID = System.Guid.NewGuid().ToString();
        }
        enemyIDCopy = EnemyID;
    }

    private void OnEnable()
    {
        StatGrowth = new Dictionary<IntStatType, float>()
        {
            { IntStatType.MaxHP, hpGrowth },
            { IntStatType.MaxMP, mpGrowth },
            { IntStatType.Attack, attackGrowth },
            { IntStatType.Defense, defenseGrowth },
            { IntStatType.MAttack, mAttackGrowth },
            { IntStatType.MDefense, mDefenseGrowth },
            { IntStatType.Agility, agiltyGrowth },
            { IntStatType.Healing, healingGrowth },
            { IntStatType.CritRate, 0 },
            { IntStatType.EvadeRate, 0 }
        };
    }

    public int GetStat(IntStatType intStatType)
    {
        return GetDerivedStatValue(intStatType);
    }

    public Dictionary<IntStatType, int> GetStatDict()
    {
        Dictionary<IntStatType, int> valuesToReturn = new Dictionary<IntStatType, int>();
        foreach (IntStatType intStatType in Enum.GetValues(typeof(IntStatType)))
        {
            valuesToReturn.Add(intStatType, GetDerivedStatValue(intStatType));
        }
        return valuesToReturn;
    }

    private int GetDerivedStatValue(IntStatType statType)
    {
        if (statType == IntStatType.MaxMP || statType == IntStatType.EvadeRate || statType == IntStatType.CritRate)
        {
            return 0;
        }

        int hpBaseValue = 5;
        int standardBaseValue = 5;

        int baseValueToUse = standardBaseValue;
        if (statType == IntStatType.MaxHP)
        {
            baseValueToUse = hpBaseValue;
        }

        int value = Mathf.FloorToInt(baseValueToUse + (Level * StatGrowth[statType]));

        //traits
        foreach (Trait trait in Traits)
        {
            foreach (IntStatModifier statModifier in trait.IntStatModifiers)
            {
                if (statModifier.IntStatType == statType)
                {
                    value += statModifier.Modifier;
                }
            }
        }

        int maxValue = 99;
        if (statType == IntStatType.MaxHP)
        {
            maxValue = 99999;
        } 

        return Mathf.Clamp(value, 1, maxValue);
    }
}
