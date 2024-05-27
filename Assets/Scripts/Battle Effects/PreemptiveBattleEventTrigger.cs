using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public enum PreemptiveTriggerCombatantType
{
    SelfOnly,
    AllyOnly, 
    Any
}

public enum PreemptiveTriggerTargetingType
{
    Actor,
    OneTarget,
    AllTargets
}

[CreateAssetMenu(fileName = "NewBattleEventTrigger", menuName = "BattleEventTrigger/Preemptive")]
public class PreemptiveBattleEventTrigger : ScriptableObject
{
    [field: Header("Trigger Conditions")]
    [SerializeField] private List<BattleConditionContainer> userTriggerConditions = new List<BattleConditionContainer>();

    [SerializeField] private List<BattleConditionContainer> actionTriggerConditions = new List<BattleConditionContainer>();
    [SerializeField][Range(0.01f, 1f)] private float triggerRate;

    [field: Header("UI Info")]
    [field: SerializeField] public string EventName { get; private set; }
    [field: SerializeField] public TriggerableEffectType TriggerableEffectType { get; private set; }

    [field: Header("Effects to Trigger")]
    [field: SerializeField] public List<TriggerableEffectContainer> TriggerableEffectContainers { get; private set; } = new List<TriggerableEffectContainer>();
    [field: SerializeField] private List<ActionEventModifier> ActionEventModifiers = new List<ActionEventModifier>();
    [field: SerializeField] public bool IsProtect = false;
    [field: SerializeField] public int ProtectPriority = 0;

    [field: Header("Targeting")]
    [field: SerializeField] public PreemptiveTriggerCombatantType PreemptiveTriggerCombatantType { get; private set; }
    [field: SerializeField] public PreemptiveTriggerTargetingType PreemptiveTriggerTargetingType { get; private set; }

    [field: Header("Animations")]
    [field: SerializeField] public List<ActionAnimationData> ActionAnimations { get; private set; } = new List<ActionAnimationData>();
    [field: SerializeField] public ExecutePosition ExecutePosition { get; private set; }

    public ActionEvent ApplyModifiers(ActionEvent actionEvent, Combatant actor, List<Combatant> targets)
    {
        foreach (ActionEventModifier actionEventModifier in ActionEventModifiers)
        {
            actionEvent = actionEventModifier.ModifyActionEvent(actionEvent, actor, targets);
        }
        return actionEvent;
    }

    public List<Combatant> GetTargets(ActionEvent actionEvent, Combatant user)
    {
        List<Combatant> actionTargets = new List<Combatant>();
        List<Combatant> battleEventTargets = new List<Combatant>();

        //get targets of upcoming action
        foreach (ActionSubevent actionSubevent in actionEvent.ActionSubevents)
        {
            actionTargets.Add(actionSubevent.Target);
        }

        //return empty list if user conditions are not met or roll fails
        if (!TriggerCheck() || !ConditionsCheck(userTriggerConditions, user, user, new ActionSummary(actionEvent.Action, actionEvent.IsIntervention)))
        {
            return battleEventTargets;
        }

        //check conditions for each target
        foreach (Combatant target in actionTargets)
        {
            if (ConditionsCheck(actionTriggerConditions, actionEvent.Actor, target, new ActionSummary(actionEvent.Action, actionEvent.IsIntervention)))
            {
                if (PreemptiveTriggerTargetingType == PreemptiveTriggerTargetingType.Actor)
                {
                    battleEventTargets.Add(actionEvent.Actor);
                    break;
                }
                else if (PreemptiveTriggerCombatantType == PreemptiveTriggerCombatantType.Any
                    || PreemptiveTriggerCombatantType == PreemptiveTriggerCombatantType.SelfOnly && target == user 
                    || PreemptiveTriggerCombatantType == PreemptiveTriggerCombatantType.AllyOnly && target != user)
                {
                    battleEventTargets.Add(target);
                }
            }
        }

        if (PreemptiveTriggerTargetingType == PreemptiveTriggerTargetingType.OneTarget && battleEventTargets.Count > 1)
        {
            int roll = Random.Range(0, battleEventTargets.Count);
            Combatant selectedTarget = battleEventTargets[roll];
            battleEventTargets = new List<Combatant> { selectedTarget };
        }

        return battleEventTargets;
    }

    public bool ConditionsCheck(List<BattleConditionContainer> triggerConditions, Combatant combatantA, Combatant combatantB, ActionSummary actionSummary)
    {
        if (actionSummary == null || actionSummary.Action == null)
        {
            Debug.Log("error: action summary/action missing");
            return false;
        }

        bool conditionsSatisfied = true;
        foreach (BattleConditionContainer battleConditionContainer in triggerConditions)
        {
            Combatant combatantToCheck = combatantA;
            if (battleConditionContainer.BattleConditionCombatant  == BattleConditionCombatant.CombatantB)
            {
                combatantToCheck = combatantB;
            }
            if (battleConditionContainer.Conditional == Conditional.If)
            {
                conditionsSatisfied = battleConditionContainer.CheckCondition(combatantToCheck, actionSummary);
            }
            else if (battleConditionContainer.Conditional == Conditional.And)
            {
                if (!conditionsSatisfied)
                {
                    continue;
                }
                else
                {
                    conditionsSatisfied = battleConditionContainer.CheckCondition(combatantToCheck, actionSummary);
                }
            }
            else if (battleConditionContainer.Conditional == Conditional.Or)
            {
                if (conditionsSatisfied)
                {
                    return true;
                }
                else
                {
                    conditionsSatisfied = battleConditionContainer.CheckCondition(combatantToCheck, actionSummary);
                }
            }
        }
        return conditionsSatisfied;
    }

    public bool TriggerCheck()
    {
        return Roll(triggerRate);
    }

    public bool Roll(float chance)
    {
        float roll = Random.Range(0.01f, 1f);
        if (roll <= chance)
        {
            return true;
        }
        else
        {
            return false;
        }
    }
}
