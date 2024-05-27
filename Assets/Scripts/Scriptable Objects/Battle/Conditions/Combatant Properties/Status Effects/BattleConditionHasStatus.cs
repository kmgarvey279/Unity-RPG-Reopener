using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu(fileName = "New Battle Condition", menuName = "Battle Conditions/Status Effects/Has Status")]
public class BattleConditionHasStatus : BattleCondition
{
    [SerializeField] private StatusEffect statusEffect;
    public override bool CheckCondition(Combatant combatantToCheck, float value, ActionSummary actionSummary = null)
    {
        foreach (StatusEffectInstance statusEffectInstance in combatantToCheck.StatusEffectInstances)
        {
            if(statusEffectInstance.StatusEffect == statusEffect)
            {
                return true;
            }
        }
        return false;
    }
}
