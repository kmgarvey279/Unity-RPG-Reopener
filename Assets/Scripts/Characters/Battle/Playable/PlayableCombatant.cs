using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayableCombatant : Combatant
{
    public Action Attack { get; private set; }
    public Action Defend { get; private set; }
    public List<Action> Skills { get; private set; } = new List<Action>();
    public BattlePartyPanel BattlePartyPanel { get; private set; }
    public PlayableCharacterID PlayableCharacterID { get; private set; }
    public PlayableCharacterID LinkedCharacterID { get; private set; }

    protected override void Awake()
    {
        base.Awake();
        CombatantType = CombatantType.Player;  
    }

    public void AssignBattlePartyPanel(BattlePartyPanel battlePartyPanel)
    {
        this.BattlePartyPanel = battlePartyPanel;
    }

    public override void SetCharacterData(CharacterInfo characterInfo, PlayableCharacterID linkedCharacterID)
    {
        base.SetCharacterData(characterInfo);
        
        PlayableCharacterInfo playableCharacterInfo = (PlayableCharacterInfo)characterInfo;
        Attack = playableCharacterInfo.Attack;
        Defend = playableCharacterInfo.Defend;
        PlayableCharacterID = playableCharacterInfo.PlayableCharacterID;
        LinkedCharacterID = linkedCharacterID;
        Skills.AddRange(playableCharacterInfo.Skills);

        IsLoaded = true;
    }

    public override void OnDamaged(int amount, bool isCrit = false, ElementalProperty elementalProperty = ElementalProperty.None)
    {
        base.OnDamaged(amount, isCrit, elementalProperty);
        BattlePartyPanel.UpdateHP();
    }
    public override void OnHealed(int amount, bool isCrit = false)
    {
        base.OnHealed(amount, isCrit);
        BattlePartyPanel.UpdateHP();
    }

    public override void ResolveHealthChange()
    {
        base.ResolveHealthChange();
        BattlePartyPanel.ResolveHP();
    }

    public void ResolveManaChange()
    {
        BattlePartyPanel.UpdateMP();
        BattlePartyPanel.ResolveMP();
    }

    public override void ApplyActionCost(ActionCostType actionCostType, int cost)
    {
        base.ApplyActionCost(actionCostType, cost);
        if (actionCostType == ActionCostType.MP)
        {
            MP.ChangeCurrentValue(-cost);

            ResolveManaChange();
        }
    }
}
