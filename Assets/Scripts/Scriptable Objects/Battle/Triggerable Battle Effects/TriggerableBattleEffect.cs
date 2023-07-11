using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public enum BattleEventType
{
    Acting, 
    Targeted,
    Turn
}

[System.Serializable]
public class TriggerableBattleEffect : ScriptableObject
{
    [Header("Conditions")]
    [SerializeField] private List<BattleConditionContainer> triggerConditions = new List<BattleConditionContainer>();
    [SerializeField] private List<ActionModifier> actionModifiers = new List<ActionModifier>();
    [Header("Trigger Rate")]
    [SerializeField][Range(0.01f,1f)] private float triggerRate;
    [field: SerializeField, Header("Effect Info")] public string EffectName { get; private set; }
    [field: SerializeField] public bool DisplayName { get; private set; }
    //trigger for the actor or target?
    [field: SerializeField] public BattleEventType BattleEventType { get; private set; }
    //does it target self?
    [field: SerializeField] public bool TargetSelf { get; private set; }
    //where is it inserted into the queue?
    [field: SerializeField] public int Priority { get; private set; } = 1;
    [field: SerializeField, Header("Animations")] public List<ActionAnimationData> ActionAnimations { get; private set; } = new List<ActionAnimationData>();
    [field: SerializeField, Header("Additional Effects")] public List<TriggerableBattleEffect> AdditionalEffects { get; private set; }
    public virtual void ApplyEffect(Combatant actor, Combatant target, ActionSummary actionSummary = null)
    {
        if (target == null)
        {
            Debug.Log("No Target Found");
            return;
        }
        Debug.Log("Applying " + EffectName + " Effect");
        foreach (TriggerableBattleEffect additionalEffect in AdditionalEffects)
        {
            additionalEffect.ApplyEffect(actor, target, actionSummary);
        }
    }

    //% chance of triggering
    public bool TriggerCheck()
    {
        return Roll(triggerRate);
    }

    //binary value: can it trigger or not
    public bool ConditionCheck(Combatant actor, Combatant target, ActionSummary actionSummary = null)
    {
        bool conditionsSatisfied = true;
        foreach (BattleConditionContainer battleConditionContainer in triggerConditions)
        {
            if (battleConditionContainer.Conditional == Conditional.If)
            {
                conditionsSatisfied = battleConditionContainer.BattleCondition.CheckCondition(actor, target, actionSummary);
                if(battleConditionContainer.IsNot)
                {
                    conditionsSatisfied = !conditionsSatisfied; 
                }
            }
            else if (battleConditionContainer.Conditional == Conditional.And)
            {
                if (!conditionsSatisfied)
                {
                    break;
                }
                else
                {
                    conditionsSatisfied = battleConditionContainer.BattleCondition.CheckCondition(actor, target, actionSummary);
                    if (battleConditionContainer.IsNot)
                    {
                        conditionsSatisfied = !conditionsSatisfied;
                    }
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
                    conditionsSatisfied = battleConditionContainer.BattleCondition.CheckCondition(actor, target, actionSummary);
                    if (battleConditionContainer.IsNot)
                    {
                        conditionsSatisfied = !conditionsSatisfied;
                    }
                }
            }
        }
        return conditionsSatisfied;
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
