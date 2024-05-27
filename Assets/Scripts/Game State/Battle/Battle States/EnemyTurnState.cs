using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System.Linq;

public class PotentialAction
{
    public int BaseWeight { get; private set; }
    public Action Action { get;  private set; }
    public int CumulativeWeight { get; private set; }
    public List<Combatant> Targets { get; private set; }

    public PotentialAction(Action _action, int _baseWeight, List<Combatant> _targets)
    {
        this.Action = _action;
        this.BaseWeight = _baseWeight;
        this.Targets = _targets;
    }

    public void SetCumulativeWeight(int weight) 
    {
        CumulativeWeight = weight;
    }
}

[System.Serializable]
public class EnemyTurnState : BattleState
{
    [SerializeField] private Action wait;
    private WaitForSeconds waitForZeroPointFive = new WaitForSeconds(0.5f);

    public override void OnEnter()
    {
        base.OnEnter();
        // onCameraZoomOut.Raise();
        StartCoroutine(SetActionCo());
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

        //get action for this turn
        PotentialAction chosenAction = GetAction();
        List<Combatant> chosenTargets;
        if (chosenAction.Action.AOEType == AOEType.All || chosenAction.Action.AOEType == AOEType.Random)
        {
            chosenTargets = chosenAction.Targets;
        }
        else
        {
            chosenTargets = new List<Combatant>() { GetTarget(chosenAction.Targets, chosenAction.Action) };
        }
        battleTimeline.CurrentTurn.SetAction(chosenAction.Action);
        battleTimeline.CurrentTurn.SetTargets(chosenTargets);

        yield return waitForZeroPointFive;
        stateMachine.ChangeState((int)BattleStateType.Execute); 
    }

    public PotentialAction GetAction()
    { 
        EnemyCombatant actor = (EnemyCombatant)battleTimeline.CurrentTurn.Actor;
        List<PotentialAction> potentialActions = new List<PotentialAction>();

        //get all enemy actions and use them to create a list of potential actions
        foreach (WeightedAction weightedAction in actor.EnemyAI.WeightedActions)
        {   
            //if action cannot be used
            if (!weightedAction.Action.IsUsable(actor))
            {
                break;
            }

            List<Combatant> targets = new List<Combatant>();
            //add self
            if (weightedAction.Action.TargetingType == TargetingType.TargetSelf)
            {
                if (!actor.CheckBool(CombatantBool.CannotTargetSelf))
                    targets.Add(actor);
            }
            //add allied targets
            else if (weightedAction.Action.TargetingType == TargetingType.TargetFriendly || weightedAction.Action.TargetingType == TargetingType.TargetAllies)
            {
                if (!actor.CheckBool(CombatantBool.CannotTargetAlly))
                {
                    List<Combatant> combatants = battleManager.GetCombatants(CombatantType.Enemy);
                    foreach (Combatant combatant in combatants)
                    {
                        if (!combatant.CheckBool(CombatantBool.CannotBeTargetedByAlly))
                            targets.Add(combatant);
                    }
                    if (weightedAction.Action.TargetingType == TargetingType.TargetAllies)
                    {
                        targets.Remove(actor);
                    }
                }
            }
            //add hostile targets
            else if (weightedAction.Action.TargetingType == TargetingType.TargetHostile)
            {
                if (!actor.CheckBool(CombatantBool.CannotTargetHostile))
                {
                    List<Combatant> combatants = battleManager.GetCombatants(CombatantType.Player);
                    foreach (Combatant combatant in combatants)
                    {
                        if (!combatant.CheckBool(CombatantBool.CannotBeTargetedByHostile))
                            targets.Add(combatant);
                    }
                }
                Debug.Log("valid hostile targets: " + targets.Count);
            }

            //check targets
            bool isApplicable = false;
            foreach (Combatant target in targets)
            {
                //is the action applicable for *any* target?
                if (ActionCheck(weightedAction.Action, target))
                {
                    isApplicable = true;
                }
            }
            //add all targets if action is applicable
            if (isApplicable)
            {
                potentialActions.Add(new PotentialAction(weightedAction.Action, weightedAction.BaseWeight(), targets));
            }
        }
        int totalWeight = 0;
        if (potentialActions.Count > 0)
        {
            foreach(PotentialAction potentialAction in potentialActions)
            {
                int actionWeight = potentialAction.BaseWeight;
                //if action was used one turn ago...
                if(actor.EnemyAI.LastActions.Count > 0 && actor.EnemyAI.LastActions[0] == potentialAction.Action)
                {
                    actionWeight = Mathf.FloorToInt(actionWeight / 3f);
                }
                //if action was used two turns ago...
                if(actor.EnemyAI.LastActions.Count > 1 && actor.EnemyAI.LastActions[1] == potentialAction.Action)
                {
                    actionWeight = Mathf.FloorToInt(actionWeight / 1.5f);
                }
                //aoes should be less likely when there is only one target
                if (potentialAction.Action.AOEType == AOEType.All && potentialAction.Targets.Count == 1)
                {
                    actionWeight = Mathf.FloorToInt(actionWeight / 2f);
                }
                //aggro
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

    private Combatant GetTarget(List<Combatant> potentialTargets, Action action)
    {
        EnemyCombatant actor = (EnemyCombatant)battleTimeline.CurrentTurn.Actor;
        float totalWeight = 0;
        Dictionary<Combatant, float> targetWeights = new Dictionary<Combatant, float>();
        foreach (Combatant target in potentialTargets) 
        {
            if (target.CheckBool(CombatantBool.MustBeTargetedByHostile))
            {
                return target;
            }
            //float weight = Mathf.Clamp(0.25f, 4f, 1 + (1 * target.FloatStats[FloatStatType.AggroMultiplierBonus].CurrentValue));
            float baseWeight = 1f;
            float targetWeight = Mathf.Clamp(ApplyWeightModifiers(baseWeight, actor, target, action), 0.25f, 10f);
            
            totalWeight += targetWeight;
            targetWeights.Add(target, totalWeight);
            Debug.Log(target.CharacterName + " weight:" + targetWeight + ", total: " + totalWeight);
        }
        potentialTargets = potentialTargets.OrderBy(potentialTarget => targetWeights[potentialTarget]).ToList();
        float roll = Random.Range(0, totalWeight);
        Debug.Log("weight roll " + roll);
        foreach (Combatant target in potentialTargets)
        {
            if (roll <= targetWeights[target])
            {
                Debug.Log("selected target: " + target.CharacterName);
                return target;
            }
        }
        Debug.Log("something went wrong, returning default target");
        return potentialTargets[0];
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
            case ActionType.ApplyDebuff:
                useAction = AddStatusEffectCheck(action, target);
                break;
            case ActionType.RemoveBuff:
            case ActionType.RemoveDebuff:
                useAction = RemoveStatusEffectsCheck(action, target);
                break;
            case ActionType.RemoveAllBuffs:
                useAction = RemoveAllStatusEffectsCheck(action, target, StatusEffectType.Buff);
                break;
            case ActionType.RemoveAllDebuffs:
                useAction = RemoveAllStatusEffectsCheck(action, target, StatusEffectType.Debuff);
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
        if (target.HP < Mathf.FloorToInt(target.MaxHP * healThreshold))
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
        for (int i = action.StatusEffectTags.Count - 1; i >= 0; i--)
        {
            // if the effect can be refreshed or hasn't been applied
            if (!target.CheckForStatus(action.StatusEffectTags[i])
                || action.StatusEffectTags[i].HasDuration && DurationRefreshCheck(target, action.StatusEffectTags[i]) 
                || action.StatusEffectTags[i].HasStacks && StacksRefreshCheck(target, action.StatusEffectTags[i]))
            {
                return true;
            }
        }
        return false;
    }

    private bool DurationRefreshCheck(Combatant target, StatusEffect statusEffect)
    {
        StatusEffectInstance statusEffectInstance = target.GetStatusEffectInstance(statusEffect);
        
        if (statusEffectInstance == null && statusEffect.CanIncreaseDuration && statusEffectInstance.Duration.CurrentValue < statusEffect.DurationMax)
        {
            return true;
        }
        return false;
    }

    private bool StacksRefreshCheck(Combatant target, StatusEffect statusEffect)
    {
        StatusEffectInstance statusEffectInstance = target.GetStatusEffectInstance(statusEffect);

        if (statusEffectInstance == null && statusEffect.CanIncreaseStacks && statusEffectInstance.Stacks.CurrentValue < statusEffect.StacksMax)
        {
            return true;
        }
        return false;
    }

    private bool RemoveStatusEffectsCheck(Action action, Combatant target)
    {
        for (int i = action.StatusEffectTags.Count - 1; i >= 0; i--)
        {
            if (target.CheckForStatus(action.StatusEffectTags[i]))
            {
                return true;
            }
        }
        return false;
    }

    private bool RemoveAllStatusEffectsCheck(Action action, Combatant target, StatusEffectType statusEffectType)
    {      
        if (target.GetStatusCount(statusEffectType, true) > 0)
        {
            return true;
        }
        return false;
    }

    private float ApplyWeightModifiers(float baseValue, Combatant actor, Combatant target, Action action)
    {
        ActionSummary actionSummary = new ActionSummary(action, false);
        //action
        foreach (ActionModifier actionModifier in action.ActionModifierDict[ActionModifierType.TargetWeight])
        {
            baseValue = actionModifier.ApplyModifier(baseValue, actor, target, actionSummary);
        }
        //action (custom)
        foreach (CustomActionModifier actionModifier in action.CustomActionModifierDict[ActionModifierType.TargetWeight])
        {
            baseValue = actionModifier.ApplyModifier(baseValue, actor, target, actionSummary);
        }
        //actor
        foreach (ActionModifier actionModifier in actor.ActionModifiers[BattleEventType.Acting][ActionModifierType.TargetWeight])
        {
            baseValue = actionModifier.ApplyModifier(baseValue, actor, target, actionSummary);
        }
        //target
        foreach (ActionModifier actionModifier in target.ActionModifiers[BattleEventType.Targeted][ActionModifierType.TargetWeight])
        {
            baseValue = actionModifier.ApplyModifier(baseValue, target, actor, actionSummary);
        }
        return baseValue;
    }
}

