using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System;
using DG.Tweening;

//spawned effect w/ hitbox (ex: spells)
public class DamageEffect : Effect
{
    public HitboxSO hitboxSO;
    private float damage;
    private string damageTag;

    public override void SetDamage(CharacterInfo characterInfo)
    {

        if(hitboxSO.isSpecial)
        {
            damage = characterInfo.special.GetValue() * hitboxSO.damageMultiplier;
        }
        else
        {
            damage = characterInfo.special.GetValue() * hitboxSO.damageMultiplier;
        }
    }

    public void OnTriggerEnter2D(Collider2D other)
    {
        if(other.gameObject.CompareTag(damageTag) && other.isTrigger)
        {
            Rigidbody2D hitObject = other.GetComponentInParent<Rigidbody2D>();
            if(hitObject != null)
            {
                Vector3 direction = hitObject.transform.position - transform.position;
                hitObject.DOMove((Vector3)hitObject.transform.position + direction.normalized * hitboxSO.knockForce, hitboxSO.knockDuration);
                other.GetComponent<Hurtbox>().TakeDamage(damage, hitboxSO);
            }
        }
    }
}
