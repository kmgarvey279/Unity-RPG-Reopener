using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace BattleCalculationsNamespace
{
    public class BattleCalculations
    {
        //get accuracy %
        public int GetHitChance(Action action, Combatant attacker, Combatant target, int distance)
        {
            if(action.guaranteedHit)
            {
                return 100;
            }
            int distancePenalty = 0;
            if(action.distancePenalty)
            {
                distancePenalty = Mathf.RoundToInt((float)distance * 2.5f);
            }
            return Mathf.Clamp(action.accuracy + attacker.battleStatDict[BattleStatType.Accuracy].GetValue() - target.battleStatDict[BattleStatType.Evasion].GetValue() - distancePenalty, 1, 100);
        }

        //roll to check if action hits target
        public bool HitCheck(Action action, Combatant attacker, Combatant target, int distance)
        {
            if(action.guaranteedHit)
            {
                return true;
            }
            int hitChance = GetHitChance(action, attacker, target, distance);
            int roll = Random.Range(1, 100);
            if(roll <= hitChance )
            {
                return true;
            }
            else
            {
                return false;
            }
        }

        //get final damage of attack
        public int GetDamageAmount(Action action, Combatant attacker, Combatant target)
        {
            float offensiveStat = 0;
            float defensiveStat = 0;
            offensiveStat = (float)attacker.battleStatDict[action.offensiveStat].GetValue();
            defensiveStat = (float)target.battleStatDict[action.defensiveStat].GetValue();
            float crit = CritCheck(attacker); 

            float damage = Mathf.Clamp((offensiveStat * (float)action.power) * (100f/(100f + defensiveStat)) * Random.Range(0.85f, 1f), 1, 9999);
            return Mathf.FloorToInt(damage);
        }

        public float CritCheck(Combatant attacker)
        {
            float critChance = attacker.battleStatDict[BattleStatType.CritRate].GetValue();
            float roll = Random.Range(1, 100);
            if(roll <= critChance)
            {
                return 2.5f;
            }
            else
            {
                return 1f;
            }
        }
    }
}
