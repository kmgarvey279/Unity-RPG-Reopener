using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[System.Serializable]
public class ActionEffectDamage : ActionEffect
{
    [SerializeField] private float power = 10f;
    [SerializeField][Range(1,100)] private float baseCritRate = 5f;
    [SerializeField] private BattleStatType offensiveStat = BattleStatType.None;
    [SerializeField] private BattleStatType defensiveStat = BattleStatType.None;
    [SerializeField] private ElementalProperty elementalProperty = ElementalProperty.None;
    public override void ApplyEffect(ActionEvent actionEvent)
    {
        float healthEffect = 0;
        //attack and defense values
        float actionPower = 0;
        float stat = actionEvent.actor.battleStatDict[offensiveStat].GetValue();
        actionPower = (power + 1f) * (stat + 1f);
        //rng
        actionPower = actionPower * Random.Range(0.90f, 1f);
        //melee close range bonus
        // if(actionEvent.action.isMelee && Mathf.Abs(actionEvent.actor.tile.x - actionEvent.targets[0].tile.x) == 1)
        // {
        //     actionPower *= 1.25f;
        // }
        //check for crit
        bool isCrit = false;
        float critMultiplier = 1;
        float critTotal = actionEvent.actor.battleStatDict[BattleStatType.CritRate].GetValue() + baseCritRate;
        if(Roll(critTotal))
        {
            isCrit = true;
            critMultiplier = 1.5f;
        }
        healthEffect = actionPower * critMultiplier;
        //defense
        float targetDefense = actionEvent.targets[0].battleStatDict[defensiveStat].GetValue() * 10f / 4f;
        healthEffect = healthEffect - targetDefense;
        //resistances
        float resistMultiplier = actionEvent.targets[0].resistDict[elementalProperty].GetResistMultiplier();
        healthEffect = healthEffect * resistMultiplier;

        actionEvent.targets[0].Damage(Mathf.Clamp(Mathf.Floor(healthEffect), 1, 9999), isCrit);
    }
}
