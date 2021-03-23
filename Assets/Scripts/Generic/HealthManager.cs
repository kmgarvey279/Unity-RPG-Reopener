using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using StateMachineNamespace;

public class HealthManager : MonoBehaviour
{
    [Header("Character Stats")]
    public CharacterStats stats;
    [Header("Character State")]
    private StateMachine stateMachine;
    public StateMachine.State stunState; 
    public StateMachine.State moveState;
    public StateMachine.State dieState;
    [Header("Death")]
    public float deathDelay = 0.3f;

    // Start is called before the first frame update
    public virtual void OnEnable()
    {
        stateMachine = GetComponentInParent<StateMachine>();   
    }

    public virtual void Heal(float healAmount)
    {
        stats.ChangeCurrentHealth(healAmount);
    }


    public virtual void TakeDamage(float damage, float stunDuration)
    {
        float finalDamage = GetFinalDamage(damage);
        stats.ChangeCurrentHealth(-finalDamage);
        StartCoroutine(StunCo(stunDuration));
    }

    private IEnumerator StunCo(float stunDuration)
    {
        stateMachine.ChangeState(stunState);
        yield return new WaitForSeconds(stunDuration);
        if(stats.currentHealth <= 0)
        {
            StartCoroutine(DeathCo());
        }
        else
        {
            stateMachine.ChangeState(moveState);   
        }
    }

    private float GetFinalDamage(float baseDamage)
    {
        float temp = baseDamage - stats.defense.GetValue();
        if(temp < 0)
        {
            temp = 0;
        }
        return temp;
    }

    public IEnumerator DeathCo()
    {
        yield return new WaitForSeconds(deathDelay);
        stateMachine.ChangeState(dieState);
    }

    
}
