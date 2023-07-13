using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System.Linq;

public class PotentialAction
{
    private int baseWeight;

    public Action Action { get;  private set; }
    public int CumulativeWeight { get; private set; }
    public List<Combatant> Targets { get; private set; }
    
    public PotentialAction(Action _action, int _baseWeight, List<Combatant> _targets)
    {
        this.Action = _action;
        this.baseWeight = _baseWeight;
        this.Targets = _targets;
    }
    public int GetWeight()
    {
        int weight = baseWeight;
        weight = weight * Mathf.Clamp(Targets.Count, 0, 3);
        return weight;
    }

    public void SetCumulativeWeight(int weight) 
    {
        CumulativeWeight = weight;
    }
}

[System.Serializable]
public class EnemyTurnState : BattleState
{
    //private bool interventionWindow = true;
    [SerializeField] private Action wait;
    private WaitForSeconds waitForZeroPointFive = new WaitForSeconds(0.5f);

    public override void OnEnter()
    {
        base.OnEnter();
        // onCameraZoomOut.Raise();
        //interventionWindow = true;
        StartCoroutine(SetActionCo());
    }

    public override void StateUpdate()
    {
        if (Input.GetButtonDown("QueueIntervention1"))
        {
            if (battleManager.InterventionCheck(0))
            {
                if (Input.GetButton("Shift"))
                {
                    battleTimeline.RemoveLastIntervention(battleManager.PlayableCombatants[0]);
                }
                else
                {
                    battleTimeline.AddInterventionToQueue(battleManager.PlayableCombatants[0]);
                }
            }
        }
        else if (Input.GetButtonDown("QueueIntervention2"))
        {
            if (battleManager.InterventionCheck(1))
            {
                if (Input.GetButton("Shift"))
                {
                    battleTimeline.RemoveLastIntervention(battleManager.PlayableCombatants[1]);
                }
                else
                {
                    battleTimeline.AddInterventionToQueue(battleManager.PlayableCombatants[1]);
                }
            }
        }
        else if (Input.GetButtonDown("QueueIntervention3"))
        {
            if (battleManager.InterventionCheck(2))
            {
                if (Input.GetButton("Shift"))
                {
                    battleTimeline.RemoveLastIntervention(battleManager.PlayableCombatants[2]);
                }
                else
                {
                    battleTimeline.AddInterventionToQueue(battleManager.PlayableCombatants[2]);
                }
            }
        }
    }

    public override void StateFixedUpdate()
    {

    }

    public override void OnExit()
    {
        base.OnExit();
    }

    private IEnumerator SetActionCo()
    {
        yield return waitForZeroPointFive;
        //interventionWindow = false;

        //get action for this turn
        PotentialAction potentialAction = GetAction();
        battleTimeline.UpdateTurnAction(battleTimeline.CurrentTurn, potentialAction.Action);
        battleTimeline.UpdateTurnTargets(battleTimeline.CurrentTurn, potentialAction.Targets);
        
        yield return waitForZeroPointFive;
        stateMachine.ChangeState((int)BattleStateType.Execute); 
    }

    public PotentialAction GetAction()
    { 
        EnemyCombatant actor = (EnemyCombatant)battleTimeline.CurrentTurn.Actor;
        List<PotentialAction> potentialActions = new List<PotentialAction>();

        //get all enemy actions and use them to create a list of potential actions
        foreach (WeightedAction weightedAction in actor.WeightedActions)
        {   
            //if action cannot be used
            if (!weightedAction.Action.IsUsable(actor))
            {
                break;
            }

            //if target self only
            if(weightedAction.Action.TargetingType == TargetingType.TargetSelf)
            {
                if (ActionCheck(weightedAction.Action, actor))
                {
                    potentialActions.Add(new PotentialAction(weightedAction.Action, weightedAction.BaseWeight(), new List<Combatant>() { actor }));
                }
                break;
            }

            List<Combatant> targets = new List<Combatant>();
            //get list of targets
            if (weightedAction.Action.TargetingType == TargetingType.TargetFriendly)
            {
                targets.AddRange(battleManager.GetCombatants(CombatantType.Enemy));
            }
            if (weightedAction.Action.TargetingType == TargetingType.TargetHostile)
            {
                targets.AddRange(battleManager.GetCombatants(CombatantType.Player));
            }

            //check targets
            bool isApplicable = false;
            foreach(Combatant target in targets)
            {
                //is the action applicable for *any* target?
                if(ActionCheck(weightedAction.Action, target))
                {
                    isApplicable = true;
                    //add target if action is single target
                    if (weightedAction.Action.AOEType != AOEType.All && isApplicable)
                    {
                        potentialActions.Add(new PotentialAction(weightedAction.Action, weightedAction.BaseWeight(), new List<Combatant>() { target }));
                    }
                }
            }
            //add all targets if action is multi-target
            if(weightedAction.Action.AOEType == AOEType.All && isApplicable)
            {
                potentialActions.Add(new PotentialAction(weightedAction.Action, weightedAction.BaseWeight(), targets));
            }
        }
        int totalWeight = 0;
        if(potentialActions.Count > 0)
        {
            foreach(PotentialAction potentialAction in potentialActions)
            {
                int actionWeight = potentialAction.GetWeight();
                //if action was used one turn ago...
                if(actor.lastActions.Count > 0 && actor.lastActions[0] == potentialAction.Action)
                {
                    actionWeight = Mathf.RoundToInt((float)actionWeight / 3f);
                }
                //if action was used two turns ago...
                if(actor.lastActions.Count > 1 && actor.lastActions[1] == potentialAction.Action)
                {
                    actionWeight = Mathf.RoundToInt((float)actionWeight / 1.5f);
                }
                totalWeight += actionWeight;
                potentialAction.SetCumulativeWeight(totalWeight);
            }
            potentialActions = potentialActions.OrderBy(potentialAction => potentialAction.CumulativeWeight).ToList();
            int roll = Random.Range(1, totalWeight);
            for(int i = 0; i < potentialActions.Count; i++)
            {
                Debug.Log(potentialActions[i].Action.ActionName + " " + potentialActions[i].CumulativeWeight + " " + roll);
                if(roll <= potentialActions[i].CumulativeWeight)
                {
                    Debug.Log("action: " + potentialActions[i].Action.ActionName);
                    return potentialActions[i];
                }
            }
        }
        return new PotentialAction(wait, 0, new List<Combatant>() { actor });
    }

    //checks if action is applicable to current situation
    private bool ActionCheck(Action action, Combatant target)
    {
        bool useAction = false;
        switch (action.ActionType)
        {
            case ActionType.Attack:
                useAction = true;
                break;
            case ActionType.Heal:
                useAction = HealCheck(action, target);
                break;
            case ActionType.ApplyBuff:
                useAction = AddStatusEffectCheck(action, target);
                break;
            case ActionType.ApplyDebuff:
                useAction = AddStatusEffectCheck(action, target);
                break;
            case ActionType.RemoveBuff:
                useAction = RemoveStatusEffectsCheck(action, target);
                break;
            case ActionType.RemoveDebuff:
                useAction = RemoveStatusEffectsCheck(action, target);
                break;
            case ActionType.Other:
                useAction = true;
                break;
            default:
                break;
        }
        return useAction;
    }

    private bool HealCheck(Action action, Combatant target)
    {
        bool isBoss = false;
        if(target is EnemyCombatant)
        {
            EnemyCombatant enemyCombatant = (EnemyCombatant)target;
            if(enemyCombatant.IsBoss)
            {
                isBoss = true;
            }
        }
        //only heal if target at less than 75% of max hp (more for bosses)
        float healThreshold = 0.75f;
        if(isBoss)
        {
            healThreshold = 0.95f;
        }
        if (target.HP.Value < Mathf.FloorToInt((float)target.HP.MaxValue * healThreshold))
        {
            return true;
        }
        else 
        {
            return false;
        }
    }

    private bool AddStatusEffectCheck(Action action, Combatant target)
    {
        List<StatusEffect> statusEffects = new List<StatusEffect>();
        foreach (TriggerableBattleEffect triggerableBattleEffect in action.TriggerableBattleEffects)
        {
            if (triggerableBattleEffect is TriggerableBattleEffectApplyStatus)
            {
                TriggerableBattleEffectApplyStatus triggerableBattleEffectApplyStatus = (TriggerableBattleEffectApplyStatus) triggerableBattleEffect;
                statusEffects.Add(triggerableBattleEffectApplyStatus.StatusEffect); 
            }
        }
        //for each status effect an action will attempt to inflict...
        for (int i = statusEffects.Count - 1; i >= 0; i--)
        {
            //for each status effect instance in the target's list...
            foreach (StatusEffectInstance statusEffectInstance in target.StatusEffectInstances)
            {
                //if status effect to apply is already active on target 
                if (statusEffectInstance.StatusEffect == statusEffects[i])
                {
                    statusEffects.RemoveAt(i);
                    break;
                }
            }
        }
        if(statusEffects.Count > 0)
        {
            return true;
        }
        else
        {
            return false;
        }
    }

    private bool RemoveStatusEffectsCheck(Action action, Combatant target)
    {
        TriggerableBattleEffectRemoveStatus triggerableBattleEffectRemoveStatus = null;
        foreach (TriggerableBattleEffect triggerableBattleEffect in action.TriggerableBattleEffects)
        {
            if (triggerableBattleEffect is TriggerableBattleEffectRemoveStatus)
            {
                triggerableBattleEffectRemoveStatus = (TriggerableBattleEffectRemoveStatus)triggerableBattleEffect;
                break;
            }
        }
        if(triggerableBattleEffectRemoveStatus != null)
        {
            //any removable buffs on playable character?
            foreach (StatusEffectInstance statusEffectInstance in target.StatusEffectInstances)
            {
                if (target is PlayableCombatant 
                        && statusEffectInstance.StatusEffect.StatusEffectType == StatusEffectType.Buff 
                        && statusEffectInstance.StatusEffect.CanRemove
                        && (triggerableBattleEffectRemoveStatus.StatusEffectsToRemove.Contains(statusEffectInstance.StatusEffect)
                        || triggerableBattleEffectRemoveStatus.RemoveAll))
                {
                    return true;
                }
                //any removable debuffs on enemy?
                else if (target is EnemyCombatant 
                    && statusEffectInstance.StatusEffect.StatusEffectType == StatusEffectType.Debuff
                    && statusEffectInstance.StatusEffect.CanRemove
                    && (triggerableBattleEffectRemoveStatus.StatusEffectsToRemove.Contains(statusEffectInstance.StatusEffect)
                        || triggerableBattleEffectRemoveStatus.RemoveAll))
                {
                    return true;
                }
            }
        }
        return false;
    }
}

