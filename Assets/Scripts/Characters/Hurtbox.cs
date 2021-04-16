using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using StateMachineNamespace;

public class Hurtbox : MonoBehaviour
{
    public CharacterInfo characterInfo;
    public StateMachine stateMachine;
    public UnitHealthDisplay healthDisplay;
    [Header("Death")]
    public float deathDelay = 0.3f;

    // Start is called before the first frame update
    public virtual void OnEnable()
    {
        characterInfo = GetComponentInParent<Character>().characterInfo; 
        healthDisplay = GetComponentInChildren<UnitHealthDisplay>();
    }

    public void HandleIncomingAttack(AttackData attackData)
    {
        if(attackData.isHeal)
        {
            Heal(attackData);
        }
        else
        {
            TakeDamage(attackData);
        }
    }

    public virtual void Heal(AttackData attackData)
    {
        float healAmount = attackData.attackPower * attackData.attackMultiplier;
        characterInfo.health.ChangeCurrentValue(healAmount);
        healthDisplay.HandleHealthChange(DamagePopupType.Heal, healAmount);
    }

    public virtual void TakeDamage(AttackData attackData)
    {
        float rawDamage = attackData.attackPower * attackData.attackMultiplier;
        //calculate damage + update health
        float finalDamage = rawDamage - characterInfo.defense.GetValue();
        ///////check attack attributes///////
        finalDamage = finalDamage * Random.Range(0.75f, 1f);
        characterInfo.health.ChangeCurrentValue(-finalDamage);
        //trigger event
        healthDisplay.HandleHealthChange(DamagePopupType.Damage, finalDamage);
        //stun effect
        StartCoroutine(StunCo(attackData.knockDuration));
    }

    public IEnumerator StunCo(float stunDuration)
    {
        stateMachine.ChangeState("StunState");
        yield return new WaitForSeconds(stunDuration);
        if(characterInfo.health.currentValue <= 0)
        {
            StartCoroutine(DeathCo());
        }
        else
        {
            stateMachine.ChangeState("BattleState");   
        }
    }

    public IEnumerator DeathCo()
    {
        yield return new WaitForSeconds(deathDelay);
        stateMachine.ChangeState("DieState");
    }
}