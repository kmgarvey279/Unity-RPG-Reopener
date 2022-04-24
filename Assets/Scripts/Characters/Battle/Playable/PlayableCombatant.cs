using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayableCombatant : Combatant
{
    public bool ko = false;
    private BattlePartyPanel battlePartyPanel;
    [HideInInspector] public List<Action> skills;

    public override void Awake()
    {
        base.Awake();
        //set actions
        PlayableCharacterInfo playableCharacterInfo = (PlayableCharacterInfo)characterInfo;
        skills = playableCharacterInfo.skills;
        //set default direction
        defaultDirection = new Vector2(1, 0);
        SetDirection(defaultDirection);
    }

    public void AssignBattlePartyPanel(BattlePartyPanel battlePartyPanel)
    {
        this.battlePartyPanel = battlePartyPanel;
    }

    public override void SetBattleStats()
    {
        battleStatDict.Add(BattleStatType.Attack, new Stat(characterInfo.statDict[StatType.Attack].GetValue() + characterInfo.statDict[StatType.EquipmentAttack].GetValue()));
        battleStatDict.Add(BattleStatType.Defense, new Stat(characterInfo.statDict[StatType.Defense].GetValue() + characterInfo.statDict[StatType.EquipmentDefense].GetValue()));

        battleStatDict.Add(BattleStatType.MagicAttack, new Stat(characterInfo.statDict[StatType.MagicAttack].GetValue() + characterInfo.statDict[StatType.EquipmentMagicAttack].GetValue()));
        battleStatDict.Add(BattleStatType.MagicDefense, new Stat(characterInfo.statDict[StatType.MagicDefense].GetValue() + characterInfo.statDict[StatType.EquipmentMagicDefense].GetValue()));

        battleStatDict.Add(BattleStatType.Accuracy, new Stat(Mathf.FloorToInt(characterInfo.statDict[StatType.Skill].GetValue() / 2)));
        battleStatDict.Add(BattleStatType.CritRate, new Stat(Mathf.FloorToInt(characterInfo.statDict[StatType.Skill].GetValue() / 3)));
        
        battleStatDict.Add(BattleStatType.Speed, new Stat(characterInfo.statDict[StatType.Agility].GetValue()));
        battleStatDict.Add(BattleStatType.Evasion, new Stat(Mathf.FloorToInt(characterInfo.statDict[StatType.Agility].GetValue() / 2)));
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

    public override void Heal(int amount)
    {
        base.Heal(amount);
        battlePartyPanel.UpdateHP(hp.GetCurrentValue());
    }

    public override void Damage(int amount, Combatant attacker = null, bool isCrit = false)
    {
        base.Damage(amount, attacker, isCrit);
        battlePartyPanel.UpdateHP(hp.GetCurrentValue());
    }

    public override IEnumerator KO()
    {
        base.KO();
        animator.SetTrigger("KO");
        yield return new WaitForSeconds(1f);
        ko = true;
    }
}
