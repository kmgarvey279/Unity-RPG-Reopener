using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu(fileName = "New Action Effect (Damage)", menuName = "Action/Action Effect(Damage")]
public class EffectDamage : ActionEffect
{
    public override void ApplyEffect(Action action, Combatant attacker, Combatant target)
    {
        target.TakeDamage(10f);
    }
}
