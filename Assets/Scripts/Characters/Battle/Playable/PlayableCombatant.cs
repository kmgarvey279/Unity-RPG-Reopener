using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayableCombatant : Combatant
{
    public BattlePartyPanel battlePartyPanel;

    public override void Awake()
    {
        base.Awake();
        //set attacks
        PlayableCharacterInfo playableCharacterInfo = (PlayableCharacterInfo)characterInfo;
        //set default direction
        SetLookDirection(new Vector2(1, 0));
    }

    public void AssignBattlePartyPanel(BattlePartyPanel battlePartyPanel)
    {
        this.battlePartyPanel = battlePartyPanel;
    }

    public override void SetBattleStats(Dictionary<StatType, Stat> statDict)
    {
        battleStatDict.Add(BattleStatType.MeleeAttack, new Stat(statDict[StatType.Attack].GetValue() + statDict[StatType.EquipmentMeleeAttack].GetValue()));
        battleStatDict.Add(BattleStatType.RangedAttack, new Stat(statDict[StatType.Attack].GetValue() + statDict[StatType.EquipmentRangedAttack].GetValue()));
        battleStatDict.Add(BattleStatType.MagicAttack, new Stat(statDict[StatType.Magic].GetValue() + statDict[StatType.EquipmentMagicAttack].GetValue()));

        battleStatDict.Add(BattleStatType.PhysicalDefense, new Stat(statDict[StatType.Defense].GetValue() + statDict[StatType.EquipmentPhysicalDefense].GetValue()));
        battleStatDict.Add(BattleStatType.MagicDefense, new Stat(statDict[StatType.MagicDefense].GetValue() + statDict[StatType.EquipmentMagicDefense].GetValue()));

        battleStatDict.Add(BattleStatType.Accuracy, new Stat(Mathf.FloorToInt(statDict[StatType.Skill].GetValue() + statDict[StatType.Agility].GetValue() / 2)));
        battleStatDict.Add(BattleStatType.Evasion, new Stat(Mathf.FloorToInt(statDict[StatType.Skill].GetValue() + statDict[StatType.Agility].GetValue() / 2)));

        battleStatDict.Add(BattleStatType.CritRate, new Stat(Mathf.FloorToInt(statDict[StatType.Skill].GetValue() / 3)));
        battleStatDict.Add(BattleStatType.Speed, new Stat(statDict[StatType.Agility].GetValue()));

        battleStatDict.Add(BattleStatType.MoveRange, new Stat(statDict[StatType.MoveRange].GetValue()));
    }
}
