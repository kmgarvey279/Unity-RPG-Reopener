using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class TargetingHitbox : MonoBehaviour
{
    public bool targetFriendly;
    public bool targetHostile; 
    public List<Combatant> targets = new List<Combatant>();

    private void OnTriggerEnter2D(Collider2D other)
    {
        if(other.gameObject.CompareTag("Combatant") && other.isTrigger)
        {
            Combatant target = other.gameObject.GetComponent<Combatant>();
            
            if((targetFriendly && target is AllyCombatant || targetHostile && target is EnemyCombatant) && !targets.Contains(target))
            {
                targets.Add(target);
                target.targetIcon.ToggleImage(true);
            }
        }
    }

    private void OnTriggerExit2D(Collider2D other)
    {
        if(other.gameObject.CompareTag("Combatant") && other.isTrigger)
        {
            Combatant target = other.gameObject.GetComponent<Combatant>();

            if(targets.Contains(target))
            {
                targets.Remove(target);
                target.targetIcon.ToggleImage(false);
            }
        }
    }
    
}
