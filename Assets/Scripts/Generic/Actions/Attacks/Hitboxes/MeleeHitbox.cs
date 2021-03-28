using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using DG.Tweening;
using System;

public class MeleeHitbox : Hitbox
{
    private CharacterInfo characterInfo;
    private Vector3 attackDirection;

    private void OnEnable()
    {
        Character character = transform.root.gameObject.GetComponent<Character>();
        characterInfo = character.characterInfo;
        attackDirection = character.lookDirection;
        if(hitboxSO.isSpecial)
        {
            damage = characterInfo.special.GetValue() * hitboxSO.damageMultiplier;
        }
        else
        {
            damage = characterInfo.attack.GetValue() * hitboxSO.damageMultiplier;
        }
    }

    public override void OnTriggerEnter2D(Collider2D other)
    {
        if(other.gameObject.CompareTag(damageTag) && other.isTrigger)
        {
            Rigidbody2D hitObject = other.GetComponentInParent<Rigidbody2D>();
            if(hitObject != null)
            {
                hitObject.DOMove((Vector3)hitObject.transform.position + attackDirection * hitboxSO.knockForce, hitboxSO.knockDuration);
                other.GetComponent<Hurtbox>().TakeDamage(damage, hitboxSO);
            }
        }
    }
}
