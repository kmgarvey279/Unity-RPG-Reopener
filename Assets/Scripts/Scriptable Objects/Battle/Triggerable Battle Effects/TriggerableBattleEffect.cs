using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public enum BattleEventType
{
    Acting, 
    Targeted,
    Turn
}

public enum TriggerFrequency
{
    PerAction,
    PerHit,
    PerTurn
}

[System.Serializable]
public class TriggerableBattleEffect : ScriptableObject
{
    [Header("Conditions")]
    [SerializeField] private List<BattleConditionContainer> battleConditions = new List<BattleConditionContainer>();
    [Header("Trigger Rate")]
    [SerializeField][Range(1,100)] private float triggerRate;
    [SerializeField] private List<ActionModifier> triggerRateModifiers = new List<ActionModifier>(); 
    [field: SerializeField, Header("Effect Info")] public string EffectName { get; private set; }
    [field: SerializeField] public bool DisplayName { get; private set; }
    //trigger for the actor or target?
    [field: SerializeField] public BattleEventType BattleEventType { get; private set; }
    [field: SerializeField] public TriggerFrequency TriggerFrequency { get; private set; }
    //does it target self?
    [field: SerializeField] public bool TargetSelf { get; private set; }
    //does the effect get stronger/weaker if there are multiple targets?
    [field: SerializeField] public float MultiTargetMultiplier { get; private set; } = 0;
    //where is it inserted into the queue?
    [field: SerializeField] public int Priority { get; private set; } = 1;
    [field: SerializeField, Header("Animations")] public List<ActionAnimationData> ActionAnimations { get; private set; } = new List<ActionAnimationData>();
    [field: SerializeField, Header("Additional Effects")] public List<TriggerableBattleEffect> AdditionalEffects { get; private set; }
    public virtual void ApplyEffect(Combatant actor, Combatant target, float power = 0)
    {
        if (target == null)
        {
            Debug.Log("No Target Found");
            return;
        }
        Debug.Log("Applying " + EffectName + " Effect");
    }

    public bool TriggerCheck(ActionSubevent actionSubevent = null)
    {
        bool conditionsSatisfied = ConditionCheck(actionSubevent);
        if(conditionsSatisfied)
        {
            float finalTriggerRate = triggerRate;
            //modifiers tied to effect
            foreach (ActionModifier actionModifier in triggerRateModifiers)
            {
                if (actionModifier.ActionModifierType == ActionModifierType.EffectTriggerRate)
                {
                    finalTriggerRate += finalTriggerRate * actionModifier.GetModifier(actionSubevent);
                }
            }
            //modifiers tied to actor
            foreach (ActionModifier actionModifier in actionSubevent.Actor.ActionModifiers[BattleEventType.Acting][ActionModifierType.EffectTriggerRate])
            {
                finalTriggerRate += finalTriggerRate * actionModifier.GetModifier(actionSubevent);
            }
            //modifiers tied to target
            foreach (ActionModifier actionModifier in actionSubevent.Target.ActionModifiers[BattleEventType.Targeted][ActionModifierType.EffectTriggerRate])
            {
                finalTriggerRate += finalTriggerRate * actionModifier.GetModifier(actionSubevent);
            }
            return Roll(finalTriggerRate);
        }
        else
        {
            return false;
        }
    }

    private bool ConditionCheck(ActionSubevent actionSubevent)
    {
        bool conditionsSatisfied = true;
        foreach (BattleConditionContainer battleConditionContainer in battleConditions)
        {
            if (battleConditionContainer.Conditional == Conditional.If)
            {
                conditionsSatisfied = battleConditionContainer.BattleCondition.CheckCondition(actionSubevent);
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
                    conditionsSatisfied = battleConditionContainer.BattleCondition.CheckCondition(actionSubevent);
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
                    conditionsSatisfied = battleConditionContainer.BattleCondition.CheckCondition(actionSubevent);
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
        int roll = Random.Range(1, 100);
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
