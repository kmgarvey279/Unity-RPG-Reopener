using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[System.Serializable]
public class EnemyCombatantBattleData : CombatantBattleData
{
    [field: Header("Enemy")]
    [field: SerializeField] public EnemyInfo EnemyInfo { get; private set; }
    [field: SerializeField] public bool IsBoss { get; private set; } = false;
    [field: SerializeField] public ClampInt Guard { get; protected set; } = new ClampInt(0, 0, 0);
    [field: SerializeField] public List<ElementalProperty> Vulnerabilities { get; protected set; } = new List<ElementalProperty>();

    public EnemyCombatantBattleData(EnemyInfo enemyInfo)
    {
        //used as ID
        EnemyInfo = enemyInfo;
        
        //basic info
        CharacterName = enemyInfo.CharacterName;
        CharacterLetter = "";
        TurnIcon = enemyInfo.TurnIcon;

        //hp/mp
        HP = new ClampInt(enemyInfo.GetStat(IntStatType.MaxHP), 0, enemyInfo.GetStat(IntStatType.MaxHP));
        Barrier = new ClampInt(0, 0, enemyInfo.GetStat(IntStatType.MaxHP));
        MP = new ClampInt(0, 0, 0);

        //stats
        foreach (IntStatType statType in System.Enum.GetValues(typeof(IntStatType)))
        {
            Stats.Add(statType, enemyInfo.GetStat(statType));
            statList.Add(new StatContainer(statType, enemyInfo.GetStat(statType)));
        }

        //empty dicts/lists
        CreateDicts();

        //enemy exclusive
        IsBoss = enemyInfo.IsBoss;
        Guard = new ClampInt(enemyInfo.Guard, 0, enemyInfo.Guard);
        Vulnerabilities = enemyInfo.Vulnerabilities;

        //traits
        foreach (Trait trait in enemyInfo.Traits)
        {
            Traits.Add(trait);
        }
        ApplyTraits();
    }
}
