using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using DG.Tweening;
using System;

//standard hitboxes (not tied to any particular attacker, ex: spikes, fire)
public class Hitbox : MonoBehaviour
{
    public HitboxSO hitboxSO;
    public float damage;
    public string damageTag;

    public virtual void OnTriggerEnter2D(Collider2D other)
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
