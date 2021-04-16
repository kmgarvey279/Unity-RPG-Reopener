using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System;
using DG.Tweening;

//spawned effect w/ hitbox (ex: spells)
public class DamageEffect : Effect
{
    public AttackData attackData;

    public void SetAttackPower(CharacterInfo characterInfo)
    {
        if(attackData.isSpecial)
        {
            attackData.attackPower = characterInfo.special.GetValue();
        }
        else
        {
            attackData.attackPower = characterInfo.attack.GetValue();
        }
    }

    public void OnTriggerEnter2D(Collider2D other)
    {
        if(other.gameObject.CompareTag(attackData.damageTag) && other.isTrigger)
        {
            Rigidbody2D hitObject = other.GetComponentInParent<Rigidbody2D>();
            if(hitObject != null)
            {
                Vector3 direction = hitObject.transform.position - transform.position;
                hitObject.DOMove((Vector3)hitObject.transform.position + direction.normalized * attackData.knockForce, attackData.knockDuration);
                other.GetComponent<Hurtbox>().TakeDamage(attackData);
            }
        }
    }
}
