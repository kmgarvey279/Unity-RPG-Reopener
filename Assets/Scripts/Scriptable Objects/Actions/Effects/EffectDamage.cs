using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using BattleCalculationsNamespace;

[CreateAssetMenu(fileName = "New Action Effect (Damage)", menuName = "Action/Action Effect(Damage")]
public class EffectDamage : ActionEffect
{
    private BattleCalculations battleCalculations = new BattleCalculations();

    public override void ApplyEffect(Action action, Combatant attacker, Combatant target)
    {
        // int damage = battleCalculations.GetDamageAmount(action, attacker, target);
        // Vector2 attackDirection = new Vector2(0,0);
        // if(action.useDirection)
        //     attackDirection = attacker.lookDirection;
        // target.TakeDamage(damage, attackDirection);
    }
}
