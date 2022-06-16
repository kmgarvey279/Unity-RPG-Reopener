using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayableCombatant : Combatant
{
    [HideInInspector] public List<Action> skills;
    private BattlePartyPanel battlePartyPanel;

    public override void Awake()
    {
        base.Awake();
        combatantType = CombatantType.Player;   
        defaultDirection = new Vector2(1, 0);
        SetDirection(defaultDirection);
    }

    public void AssignBattlePartyPanel(BattlePartyPanel battlePartyPanel)
    {
        this.battlePartyPanel = battlePartyPanel;
    }

    public override void SetCharacterData(CharacterInfo characterInfo)
    {
        base.SetCharacterData(characterInfo);

        battleStatDict.Add(BattleStatType.Attack, new Stat(characterInfo.statDict[StatType.Attack].GetValue()));
        battleStatDict.Add(BattleStatType.Defense, new Stat(characterInfo.statDict[StatType.Defense].GetValue()));

        battleStatDict.Add(BattleStatType.MagicAttack, new Stat(characterInfo.statDict[StatType.MagicAttack].GetValue()));
        battleStatDict.Add(BattleStatType.MagicDefense, new Stat(characterInfo.statDict[StatType.MagicDefense].GetValue()));
        
        PlayableCharacterInfo playableCharacterInfo = (PlayableCharacterInfo)characterInfo;
        skills = playableCharacterInfo.skills;

        isLoaded = true;
    }

    // public override void SetTraitEffects()
    // {
    //     // foreach(Trait trait in characterInfo.traits)
    //     // {
    //     //     if(trait.isUnlocked)
    //     //     {
    //     //         foreach(TriggerableSubEffect triggerableSubEffect in trait.triggerableSubEffects)
    //     //         {
    //     //             triggerableSubEffects.Add(triggerableSubEffect);
    //     //         }
    //     //     }
    //     // }
    // }

    public override void Damage(float amount, bool isCrit = false)
    {
        base.Damage(amount, isCrit);
        battlePartyPanel.UpdateHP(hp.GetCurrentValue());
    }
    public override void Heal(float amount, bool isCrit = false)
    {
        base.Heal(amount, isCrit);
        battlePartyPanel.UpdateHP(hp.GetCurrentValue());
    }

    public override void ResolveHealthChange()
    {
        base.ResolveHealthChange();
        battlePartyPanel.ResolveHP();
    }

    public void ChangeMana(float amount)
    {
        mp.ChangeCurrentValue(amount);
        battlePartyPanel.UpdateMP(mp.GetCurrentValue());
    }

    public override IEnumerator KOCo()
    {
        animator.SetTrigger("KO");
        yield return new WaitForSeconds(0.5f);
    }
}
