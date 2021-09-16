using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using BattleCalculationsNamespace;

[CreateAssetMenu(fileName = "New Action Effect (Status)", menuName = "Action/Action Effect (Status)")]
public class EffectStatus : ActionEffect
{
    [SerializeField] private StatusEffect statusEffect;
    private BattleCalculations battleCalculations = new BattleCalculations();

    public override void ApplyEffect(Action action, Combatant attacker, Combatant target)
    {
        target.AddStatusEffect(statusEffect);
    }
}
