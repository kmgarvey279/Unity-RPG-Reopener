using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using DG.Tweening;

public class HitboxManager : MonoBehaviour
{
    public AttackStats attack;
    public float baseDamage;
    public string otherTag;

    public virtual void OnTriggerEnter2D(Collider2D other)
    {
        if(other.gameObject.CompareTag(otherTag) && other.isTrigger)
        {
            Rigidbody2D hitObject = other.GetComponentInParent<Rigidbody2D>();
            if(hitObject != null)
            {
                Vector3 direction = hitObject.transform.position - transform.position;
                hitObject.DOMove((Vector3)hitObject.transform.position + direction.normalized * attack.knockForce, attack.knockDuration);
                other.GetComponent<HealthManager>().TakeDamage(CalculateDamage(), attack.stunDuration);
            }
        }
    }

    public virtual float CalculateDamage()
    {
        return baseDamage; 
    }
}
