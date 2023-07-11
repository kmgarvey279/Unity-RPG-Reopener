using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu(fileName = "New Battle Condition", menuName = "Battle Conditions/Has Status")]
public class BattleConditionHasStatus : BattleCondition
{
    [SerializeField] private bool checkActor;
    [SerializeField] private StatusEffect statusEffect;
    public override bool CheckCondition(Combatant actor, Combatant target, ActionSummary actionSummary = null)
    {
        Combatant combatantToCheck = target;
        if (checkActor)
        {
            combatantToCheck = actor;
        }
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
