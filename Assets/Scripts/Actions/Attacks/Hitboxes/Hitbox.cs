using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using DG.Tweening;
using System;

public class Hitbox : MonoBehaviour
{
    public AttackData attackData;

    public virtual void OnTriggerEnter2D(Collider2D other)
    {
        if(other.gameObject.CompareTag(attackData.damageTag) && other.isTrigger)
        {
            Rigidbody2D hitObject = other.GetComponentInParent<Rigidbody2D>();
            if(hitObject != null)
            {
                hitObject.DOMove((Vector3)hitObject.transform.position + attackData.direction.normalized * attackData.knockForce, attackData.knockDuration);
                other.GetComponent<Hurtbox>().HandleIncomingAttack(attackData);
            }
        }
    }
}
