using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace BattleCalculationsNamespace
{
    public class BattleCalculations
    {
        //roll to check if action hits target
    //     public bool HitCheck(int hitChance)
    //     {
    //         int roll = Random.Range(1, 100);
    //         if(roll <= hitChance)
    //         {
    //             return true;
    //         }
    //         else
    //         {
    //             return false;
    //         }
    //     }

    //     //get final damage of attack
    //     public int GetDamageAmount(Action action, Combatant attacker, Combatant target)
    //     {
    //         float offensiveStat = 0;
    //         float defensiveStat = 0;
    //         offensiveStat = (float)attacker.battleStatDict[action.offensiveStat].GetValue();
    //         defensiveStat = (float)target.battleStatDict[action.defensiveStat].GetValue();
    //         float crit = CritCheck(attacker); 

    //         float damage = Mathf.Clamp(((float)action.power * (offensiveStat / offensiveStat + defensiveStat)) * Random.Range(0.85f, 1f), 1, 9999);
    //         return Mathf.FloorToInt(damage);
    //     }

    //     public int GetHealAmount(Action action, Combatant healer)
    //     {
    //         float healingStat = (float)healer.battleStatDict[action.offensiveStat].GetValue();
    //         float heal = Mathf.Clamp((healingStat * (float)action.power) * Random.Range(0.85f, 1f), 1, 9999);
    //         return Mathf.FloorToInt(heal);
    //     }

    //     public float CritCheck(Combatant attacker)
    //     {
    //         float critChance = attacker.battleStatDict[BattleStatType.CritRate].GetValue();
    //         float roll = Random.Range(1, 100);
    //         if(roll <= critChance)
    //         {
    //             return 2.5f;
    //         }
    //         else
    //         {
    //             return 1f;
    //         }
    //     }
    }
}
