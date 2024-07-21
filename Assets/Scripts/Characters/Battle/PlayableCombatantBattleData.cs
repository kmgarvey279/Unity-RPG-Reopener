using System.Collections;
using System.Collections.Generic;
using Unity.VersionControl.Git;
using UnityEngine;

[System.Serializable]
public class PlayableCombatantBattleData : CombatantBattleData
{
    [field: Header("Playable")]
    [field: SerializeField] public PlayableCharacterID PlayableCharacterID { get; set; }
    [field: SerializeField] public Color CharacterColor { get; set; }
    [field: SerializeField] public Action Attack { get; set; }
    [field: SerializeField] public Action Defend { get; set; }
    [field: SerializeField] public List<Action> Skills { get; set; } = new List<Action>();
    [field: SerializeField] public ClampInt IP { get; set; }
    [field: SerializeField] public bool InterventionQueued { get; set; } 
    [field: SerializeField] public bool InterventionUsed { get; set; }
    private readonly int ipMax = 50;

    public PlayableCombatantBattleData(PlayableCombatantRuntimeData data)
    {
        //ID
        PlayableCharacterID = data.PlayableCharacterID;

        //basic info
        CharacterName = data.StaticInfo.CharacterName;
        CharacterLetter = "";
        TurnIcon = data.StaticInfo.TurnIcon;
        CharacterColor = data.StaticInfo.CharacterColor;
        Level = data.Level;

        //hp/mp/IP
        HP = new ClampInt(data.CurrentHP, 0, data.GetStat(IntStatType.MaxHP));
        Barrier = new ClampInt(0, 0, data.GetStat(IntStatType.MaxHP));
        MP = new ClampInt(data.CurrentMP, 0, data.GetStat(IntStatType.MaxMP));
        IP = new ClampInt(data.CurrentIP, 0, ipMax);

        //stats
        foreach (IntStatType statType in System.Enum.GetValues(typeof(IntStatType)))
        {
            Stats.Add(statType, data.GetStat(statType));
            statList.Add(new StatContainer(statType, data.GetStat(statType)));
        }

        //empty dicts/lists
        CreateDicts();

        //skills
        Attack = data.StaticInfo.Attack;
        Defend = data.StaticInfo.Defend;
        foreach (Action action in data.UnlockedSkills)
        {
            Skills.Add(action);
        }

        //traits
        foreach (Trait trait in data.UnlockedTraits)
        {
            Traits.Add(trait);
        }
        ApplyTraits();
    }
}
