using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu(fileName = "NewActionEffectHealth", menuName = "Action/ActionEffect/Health")]
public class ActionEffectHealth : ActionEffect
{
    public override void ApplyEffect(ActionEvent actionEvent)
    {
        ApplyHealthEffect(actionEvent);
    }

    private void ApplyHealthEffect(ActionEvent actionEvent)
    {
        PopupType popupType = PopupType.Heal;
        float healthEffect = 0;
        //attack and defense values
        float actionPower = 0;
        float stat = actionEvent.actor.battleStatDict[actionEvent.action.offensiveStat].GetValue();
        actionPower = (actionEvent.action.power + 1f) * (stat + 1f);
        //rng
        if(actionEffectType == ActionEffectType.Damage)
        {
            actionPower = actionPower * Random.Range(0.90f, 1f);
        }
        //melee close range bonus
        if(actionEvent.action.isMelee && Mathf.Abs(actionEvent.actor.tile.x - actionEvent.targets[0].tile.x) == 1)
        {
            actionPower *= 1.25f;
        }
        //check for crit
        bool isCrit = false;
        float critMultiplier = 1;
        float critRate = actionEvent.actor.battleStatDict[BattleStatType.CritRate].GetValue();
        if(Roll(critRate))
        {
            isCrit = true;
            critMultiplier = 1.5f;
        }
        healthEffect = actionPower * critMultiplier;
        //defense and resistances
        if(actionEffectType == ActionEffectType.Damage)
        {
            //defense
            float targetDefense = actionEvent.targets[0].battleStatDict[actionEvent.action.defensiveStat].GetValue() * 10f / 4f;
            healthEffect = healthEffect - targetDefense;
            //resistances
            float resistMultiplier = actionEvent.targets[0].resistDict[actionEvent.action.elementalProperty].GetResistMultiplier();
            healthEffect = healthEffect * resistMultiplier;

            actionEvent.targets[0].Damage(Mathf.Clamp(Mathf.Floor(healthEffect), 1, 9999), isCrit);
        }
        else
        {
            actionEvent.targets[0].Heal(Mathf.Clamp(Mathf.Floor(healthEffect), 1, 9999), isCrit);
        }
        //apply effect
    }

    public bool Roll(float chance)
    {
        float roll = Random.Range(1, 100);
        if(roll <= chance)
        {
            return true;
        }
        else
        {
            return false;
        }
    }
}
