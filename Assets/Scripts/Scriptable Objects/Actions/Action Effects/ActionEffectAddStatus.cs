using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu(fileName = "NewActionEffectAddStatus", menuName = "Action/ActionEffect/Add Status")]
public class ActionEffectAddStatus : ActionEffect
{
    public float bossModifier;

    public override void ApplyEffect(ActionEvent actionEvent)
    {
        ApplyStatusEffect(actionEvent);
    }

    private void ApplyStatusEffect(ActionEvent actionEvent)
    {
        // float potency = 0;
        // if(actionEvent.action.statusEffectSO.healthEffect != null)
        // {
        //     if(actionEvent.action.statusEffectSO.healthEffect.doFixedPercentage)
        //     {       
        //         float percentageModifier = 1;
        //         if(actionEvent.targets[0] is EnemyCombatant)
        //         {
        //             EnemyCombatant enemyCombatant = (EnemyCombatant)actionEvent.targets[0];
        //             if(enemyCombatant.isBoss)
        //             {
        //                 percentageModifier = bossModifier;
        //             }
        //         }
        //         potency = (float)actionEvent.targets[0].hp.GetValue() * (actionEvent.action.statusEffectSO.healthEffect.fixedPercentage / percentageModifier);
        //     }
        //     else
        //     {
        //         float stat = actionEvent.actor.battleStatDict[actionEvent.action.offensiveStat].GetValue();
        //         potency = (actionEvent.action.power + 1f) * (stat + 1f);
        //     }
        // }
        actionEvent.targets[0].AddStatusEffect(actionEvent.action.statusEffectSO);
    }
}
