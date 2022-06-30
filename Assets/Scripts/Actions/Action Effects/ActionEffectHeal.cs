using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[System.Serializable]
public class ActionEffectHeal : ActionEffect
{
    [SerializeField] private float power = 10f;
    [SerializeField][Range(1,100)] private float baseCritRate = 5f;
    [SerializeField] private BattleStatType offensiveStat = BattleStatType.None;

    public override void ApplyEffect(ActionEvent actionEvent)
    {
        float healthEffect = 0;
        //attack and defense values
        float actionPower = 0;
        float stat = actionEvent.actor.battleStatDict[offensiveStat].GetValue();
        actionPower = (power + 1f) * (stat + 1f);

        bool isCrit = false;
        float critMultiplier = 1;
        float critTotal = actionEvent.actor.battleStatDict[BattleStatType.CritRate].GetValue() + baseCritRate;
        if(Roll(critTotal))
        {
            isCrit = true;
            critMultiplier = 1.5f;
        }
        healthEffect = actionPower * critMultiplier;
        actionEvent.targets[0].Heal(Mathf.Clamp(Mathf.Floor(healthEffect), 1, 9999), isCrit);
    }
}
