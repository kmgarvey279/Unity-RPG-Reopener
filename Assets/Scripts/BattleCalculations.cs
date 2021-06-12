using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace BattleCalculationsNamespace
{
    public class BattleCalculations
    {
        //check if action hits target
        public bool HitCheck(Combatant attacker, Action action, Combatant target)
        {
            int hitChance = Mathf.Clamp(action.accuracy + attacker.GetStatValue(StatType.Agility) - target.GetStatValue(StatType.Agility), 1, 99);
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
        public int GetDamageAmount(Combatant attacker, Action action, Combatant target)
        {
            float attackTotal = ((float)attacker.GetStatValue(StatType.Attack) * action.power); 
            float defenseTotal = (float)target.GetStatValue(StatType.Defense);
            float crit = CritCheck(); 

            float damage = (((attackTotal - (defenseTotal/2)) * Random.Range(0.85f, 1f)) * crit) * Random.Range(0.85f, 1f);
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
