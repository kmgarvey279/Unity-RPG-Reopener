using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using static UnityEngine.ParticleSystem;

[CreateAssetMenu(fileName = "NewPlayableCombatantStaticInfo", menuName = "Combatant/Static Info")]
public class PlayableCombatantStaticInfo : ScriptableObject
{
    [field: Header("Basic Info")]
    [field: SerializeField] public PlayableCharacterID PlayableCharacterID { get; private set; }
    [field: SerializeField] public string CharacterName { get; protected set; } = "";
    [field: SerializeField] public Color CharacterColor { get; private set; }
    [field: SerializeField] public Sprite TurnIcon { get; protected set; }
    [field: SerializeField] public GameObject Prefab { get; private set; }
    [field: Header("Stats")]
    [SerializeField] private float hpGrowth;
    [SerializeField] private float mpGrowth;
    [SerializeField] private float attackGrowth;
    [SerializeField] private float defenseGrowth;
    [SerializeField] private float mAttackGrowth;
    [SerializeField] private float mDefenseGrowth;
    [SerializeField] private float agiltyGrowth;
    [SerializeField] private float healingGrowth;
    public Dictionary<IntStatType, float> StatGrowth { get; protected set; }
    [field: Header("Traits")]
    [field: SerializeField] public List<Trait> Traits { get; protected set; } = new List<Trait>();

    [field: Header("Basic Actions")]
    [field: SerializeField] public Attack Attack { get; private set; }
    [field: SerializeField] public Action Defend { get; private set; }
    
    [field: Header("Skills")]
    [field: SerializeField] public List<Action> Skills { get; private set; } = new List<Action>();

    protected void OnEnable()
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
