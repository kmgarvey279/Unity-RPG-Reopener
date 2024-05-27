using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.UIElements;

public class PlayableCombatant : Combatant
{
    [field: SerializeField] public BattlePartyPanel BattlePartyPanel { get; private set; }

    [SerializeField] private SignalSenderInt onChangeInterventionPoints;
    private readonly int interventionCost = 25;

    #region Data Get/Set
    private PlayableCombatantBattleData playableCombatantBattleData
    {
        get
        {
            return (PlayableCombatantBattleData)combatantBattleData;
        }
    }
    public PlayableCharacterID PlayableCharacterID 
    { 
        get
        {
            return playableCombatantBattleData.PlayableCharacterID;
        }
    }
    public Color CharacterColor
    {
        get
        {
            return playableCombatantBattleData.CharacterColor;
        }
    }
    public Action Attack
    {
        get
        {
            return playableCombatantBattleData.Attack;
        }
    }
    public Action Defend
    {
        get
        {
            return playableCombatantBattleData.Defend;
        }
    }
    public List<Action> Skills
    {
        get
        {
            return playableCombatantBattleData.Skills;
        }
    }
    public int IP
    {
        get
        {
            return playableCombatantBattleData.IP.CurrentValue;
        }
        private set
        {
            playableCombatantBattleData.IP.UpdateValue(value);
        }
    }
    public int MaxIP
    {
        get
        {
            return playableCombatantBattleData.IP.MaxValue;
        }
    }

    public bool InterventionQueued
    {
        get
        {
            return playableCombatantBattleData.InterventionQueued;
        }
        private set
        {
            playableCombatantBattleData.InterventionQueued = value;
        }
    }

    public bool InterventionSpent
    {
        get
        {
            return playableCombatantBattleData.InterventionUsed;
        }
        private set
        {
            playableCombatantBattleData.InterventionUsed = value;
        }
    }
    #endregion

    protected override void Awake()
    {
        base.Awake();
        CombatantType = CombatantType.Player;
    }

    public void AssignBattlePartyPanel(BattlePartyPanel battlePartyPanel)
    {
        this.BattlePartyPanel = battlePartyPanel;
    }

    public void SetCharacterData(PlayableCombatantRuntimeData combatantData)
    {
        combatantBattleData = new PlayableCombatantBattleData(combatantData);

        healthDisplay.SetValues();
        
        IsLoaded = true;
    }

    #region Interventions
    public void SetInterventionPoints(int value)
    {
        IP = value;

        BattlePartyPanel.DisplayInterventionChanges();
    }

    public void GainInterventionPoints(int value)
    {
        IP += value;

        BattlePartyPanel.DisplayInterventionChanges();
    }

    public void SpendInterventionNode()
    {
        IP -= interventionCost;
        InterventionSpent = true;
        InterventionQueued = false;

        BattlePartyPanel.DisplayInterventionChanges();
    }

    public bool InterventionCheck()
    {
        if (CombatantState == CombatantState.Default 
            && !CheckBool(CombatantBool.CannotQueueIntervention) 
            && !InterventionQueued)
        {
            return true;
        }
        return false;
    }

    public void QueueIntervention()
    {
        InterventionQueued = true;

        BattlePartyPanel.DisplayInterventionChanges();
    }

    public void UnqueueIntervention()
    {
        InterventionQueued = false;

        BattlePartyPanel.DisplayInterventionChanges();
    }
    #endregion

    #region Events
    public override void OnTurnStart()
    { 
        base.OnTurnStart();
    }

    public override void OnAttacked(int amount, bool isCrit, bool isVulnerable)
    {
        //if (Guard.CurrentValue > 0)
        //{
        //    TriggerAnimation("Guard", false);
        //}
        //else
        //{
        TriggerAnimation("Hit", false);

        OnDamaged(amount, isCrit, false);
    }

    public void OnGainMana(int change)
    {
        MP += change;

        BattlePartyPanel.DisplayChanges();
        healthDisplay.DisplayNumberPopup(PopupType.Mana, CombatantType.Player, change);
    }

    public void OnSpendMana(int change)
    {
        MP -= change;

        BattlePartyPanel.DisplayChanges();
    }

    public override void OnKO()
    {
        base.OnKO();
        for (int i = StatusEffectInstances.Count - 1; i >= 0; i--)
        {
            StatusEffectInstance statusEffectInstance = StatusEffectInstances[i];
            if (!statusEffectInstance.StatusEffect.DoNotRemoveOnKO)
            {
                RemoveStatusEffectInstance(statusEffectInstance, false);
            }
        }
        BattlePartyPanel.OnKO();
    }

    public override void OnRevive(float percentOfHPToRestore)
    {
        base.OnRevive(percentOfHPToRestore);
        BattlePartyPanel.OnRevive();
    }
    #endregion

    #region HP/MP
    protected override void DisplayBarChanges()
    {
        base.DisplayBarChanges();
        BattlePartyPanel.DisplayChanges();
    }

    public override void ResolveBarChanges()
    {
        base.ResolveBarChanges();
        BattlePartyPanel.ResolveChanges();
    }

    public override void ApplyActionHPCost(float percentageCost)
    {
        base.ApplyActionHPCost(percentageCost);
    }
    #endregion

    #region Status Effects
    public override void HandleNewStatus(StatusEffect newStatusEffect, int potency)
    {
        base.HandleNewStatus(newStatusEffect, potency);

        if (newStatusEffect.BoolsToModify.Contains(CombatantBool.CannotQueueIntervention))
        {
            bool isTrue = combatantBattleData.CheckBool(CombatantBool.CannotQueueIntervention);
            if (isTrue)
            {
                battleTimeline.RemoveAllInterventions(this);
                //BattlePartyPanel.LockInterventionTriggerIcon(true);
            }
            else if (InterventionCheck())
            {
                //BattlePartyPanel.LockInterventionTriggerIcon(false);
            }
        }
    }

    protected override void RemoveStatusEffectInstance(StatusEffectInstance statusEffectInstance, bool wasRemovedExternally)
    {
        base.RemoveStatusEffectInstance(statusEffectInstance, wasRemovedExternally);
        
        if (statusEffectInstance.StatusEffect.BoolsToModify.Contains(CombatantBool.CannotQueueIntervention))
        {
            //BattlePartyPanel.LockInterventionTriggerIcon(false);
        }

    }
    #endregion

    #region State/Bools
    public override void ChangeCombatantState(CombatantState newState)
    {
        base.ChangeCombatantState(newState);
    }
    #endregion

    #region Status Panel
    public override void ToggleHighlight(bool isHighlighted)
    {
        base.ToggleHighlight(isHighlighted);
        if (isHighlighted)
        {
            BattlePartyPanel.OnTarget();
        }
        else
        {
            BattlePartyPanel.OnUntarget();
        }
    }
    #endregion
}
