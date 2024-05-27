using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CombatantStaticInfo : ScriptableObject
{
    [field: SerializeField] public string CharacterName { get; protected set; } = "";
    [field: SerializeField] public Sprite TurnIcon { get; protected set; }
    [field: SerializeField] public Color CharacterColor { get; protected set; }
    public Dictionary<IntStatType, int> StatStartValues { get; protected set; }

    [SerializeField] private float hpGrowth;
    [SerializeField] private float mpGrowth;
    [SerializeField] private float attackGrowth;
    [SerializeField] private float defenseGrowth;
    [SerializeField] private float mAttackGrowth;
    [SerializeField] private float mDefenseGrowth;
    [SerializeField] private float agiltyGrowth;
    [SerializeField] private float healingGrowth;
    public Dictionary<IntStatType, float> StatGrowth { get; protected set; }
    [field: SerializeField] public List<Trait> Traits { get; protected set; } = new List<Trait>();

    protected virtual void OnEnable()
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
}
