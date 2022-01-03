using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayableCombatant : Combatant
{
    public BattlePartyPanel battlePartyPanel;
    public List<Action> skills;
    public Action meleeAttack;
    public Action rangedAttack;

    public override void Awake()
    {
        base.Awake();
        //set actions
        PlayableCharacterInfo playableCharacterInfo = (PlayableCharacterInfo)characterInfo;
        meleeAttack = playableCharacterInfo.meleeAttack;
        rangedAttack = playableCharacterInfo.rangedAttack;
        skills = playableCharacterInfo.skills;
        //set default direction
        SetDirection(new Vector2(1, 0));
    }

    public override void Start()
    {
        base.Start();
    }

    public void AssignBattlePartyPanel(BattlePartyPanel battlePartyPanel)
    {
        this.battlePartyPanel = battlePartyPanel;
    }

    public override void SetBattleStats()
    {
        battleStatDict.Add(BattleStatType.MeleeAttack, new Stat(characterInfo.statDict[StatType.Attack].GetValue() + characterInfo.statDict[StatType.EquipmentMeleeAttack].GetValue()));
        battleStatDict.Add(BattleStatType.RangedAttack, new Stat(characterInfo.statDict[StatType.Attack].GetValue() + characterInfo.statDict[StatType.EquipmentRangedAttack].GetValue()));
        battleStatDict.Add(BattleStatType.MagicAttack, new Stat(characterInfo.statDict[StatType.Magic].GetValue() + characterInfo.statDict[StatType.EquipmentMagicAttack].GetValue()));

        battleStatDict.Add(BattleStatType.PhysicalDefense, new Stat(characterInfo.statDict[StatType.Defense].GetValue() + characterInfo.statDict[StatType.EquipmentPhysicalDefense].GetValue()));
        battleStatDict.Add(BattleStatType.MagicDefense, new Stat(characterInfo.statDict[StatType.MagicDefense].GetValue() + characterInfo.statDict[StatType.EquipmentMagicDefense].GetValue()));

        battleStatDict.Add(BattleStatType.Accuracy, new Stat(Mathf.FloorToInt(characterInfo.statDict[StatType.Skill].GetValue() + characterInfo.statDict[StatType.Agility].GetValue() / 2)));
        battleStatDict.Add(BattleStatType.Evasion, new Stat(Mathf.FloorToInt(characterInfo.statDict[StatType.Skill].GetValue() + characterInfo.statDict[StatType.Agility].GetValue() / 2)));

        battleStatDict.Add(BattleStatType.CritRate, new Stat(Mathf.FloorToInt(characterInfo.statDict[StatType.Skill].GetValue() / 3)));
        battleStatDict.Add(BattleStatType.Speed, new Stat(characterInfo.statDict[StatType.Agility].GetValue()));

        battleStatDict.Add(BattleStatType.MoveRange, new Stat(characterInfo.statDict[StatType.MoveRange].GetValue()));
    }

    public override void SetTraitEffects()
    {
        // foreach(Trait trait in characterInfo.traits)
        // {
        //     if(trait.isUnlocked)
        //     {
        //         foreach(TriggerableSubEffect triggerableSubEffect in trait.triggerableSubEffects)
        //         {
        //             triggerableSubEffects.Add(triggerableSubEffect);
        //         }
        //     }
        // }
    }
}
