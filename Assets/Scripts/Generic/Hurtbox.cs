using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using StateMachineNamespace;

public class Hurtbox : MonoBehaviour
{
    public Character character;
    public UnitHealthDisplay healthDisplay;
    [Header("Death")]
    public float deathDelay = 0.3f;

    // Start is called before the first frame update
    public virtual void OnEnable()
    {
        character = GetComponentInParent<Character>(); 
        healthDisplay = GetComponentInChildren<UnitHealthDisplay>();
    }

    public virtual void Heal(float healAmount)
    {
        character.characterInfo.ChangeCurrentHealth(healAmount);
        healthDisplay.HandleHealthChange(DamagePopupType.Heal, healAmount);
    }

    public virtual void TakeDamage(float damage, HitboxSO hitboxSO)
    {
        //calculate damage + update health
        float finalDamage = GetFinalDamage(damage);
        character.characterInfo.ChangeCurrentHealth(-finalDamage);
        //trigger event
        healthDisplay.HandleHealthChange(DamagePopupType.Damage, finalDamage);
        //stun effect
        StartCoroutine(StunCo(hitboxSO.stunDuration));
    }

    public IEnumerator StunCo(float stunDuration)
    {
        character.stateMachine.ChangeState(character.stunState);
        yield return new WaitForSeconds(stunDuration);
        if(character.characterInfo.currentHealth <= 0)
        {
            StartCoroutine(DeathCo());
        }
        else
        {
            character.stateMachine.ChangeState(character.moveState);   
        }
    }

    public float GetFinalDamage(float baseDamage)
    {
        float temp = baseDamage - character.characterInfo.defense.GetValue();
        return temp * Random.Range(0.75f, 1f);
    }

    public IEnumerator DeathCo()
    {
        yield return new WaitForSeconds(deathDelay);
        character.stateMachine.ChangeState(character.dieState);
    }

    
}
