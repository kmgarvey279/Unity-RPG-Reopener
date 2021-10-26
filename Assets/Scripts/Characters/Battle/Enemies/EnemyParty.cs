using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class EnemyParty : MonoBehaviour
{
    public List<Combatant> combatants = new List<Combatant>();

    private void Awake() 
    {
        BattleManager battleManager = GetComponentInParent<BattleManager>();
        foreach (Combatant combatant in combatants)
        {
            battleManager.AddEnemyCombatant(combatant);
        }    
    }
}
