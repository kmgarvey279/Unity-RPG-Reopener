using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace BattleCalculationsNamespace
{
    public class BattleCalculations
    {
        //get accuracy %
        public int GetHitChance(int actionAccuracy, int attackerAccuracy, int targetEvasion)
        {
            return Mathf.Clamp(actionAccuracy + (attackerAccuracy - targetEvasion), 1, 100);
        }

        //roll to check if action hits target
        public bool HitCheck(int actionAccuracy, int attackerAccuracy, int targetEvasion)
        {
            int hitChance = GetHitChance(actionAccuracy, attackerAccuracy, targetEvasion);
            int roll = Random.Range(1, 100);
            if(roll >= hitChance )
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
            if(action.isSpecial)
            {
                offensiveStat = (float)attacker.GetStatValue(StatType.Special);
                defensiveStat = (float)target.GetStatValue(StatType.Special);
            }
            else
            {
                offensiveStat = (float)attacker.GetStatValue(StatType.Attack);
                defensiveStat = (float)target.GetStatValue(StatType.Defense);
            }
            float crit = CritCheck(); 

            float damage = (offensiveStat * (float)action.power) * (100f/(100f + defensiveStat)) * Random.Range(0.85f, 1f);
            return Mathf.FloorToInt(damage);
        }

        public float CritCheck()
        {
            float critChance = 5;
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
