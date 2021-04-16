using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using DG.Tweening;
using System;

public class Projectile : MonoBehaviour
{
    public Rigidbody2D projectileRB;
    public GameObject impactPrefab;
    [Header("Speed")]
    public float speed;
    [Header("Lifetime")]
    public float lifetime;
    [HideInInspector] public float lifetimeCounter;
    public AttackData attackData;

    // Start is called before the first frame update
    void Awake()
    {
        projectileRB = GetComponent<Rigidbody2D>();
        lifetimeCounter = lifetime;
    }

    public virtual void Update()
    {
        lifetimeCounter -= Time.deltaTime;
        if(lifetimeCounter <= 0)
        {
            Destroy(this.gameObject);
        }
    }

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

    public void Launch(Vector3 direction)
    {
        attackData.direction = direction;
        projectileRB.velocity = direction * speed;
        // transform.rotation = Quaternion.Euler(direction);
    }

    private void OnCollisionEnter2D(Collision2D other)
    {        
        Destroy(this.gameObject);
        Instantiate(impactPrefab, transform.position, Quaternion.identity);
    }

    public virtual void OnTriggerEnter2D(Collider2D other)
    {
        if(other.gameObject.CompareTag(attackData.damageTag) && other.isTrigger)
        {
            Rigidbody2D hitObject = other.GetComponentInParent<Rigidbody2D>();
            if(hitObject != null)
            {
                hitObject.DOMove((Vector3)hitObject.transform.position + attackData.direction * attackData.knockForce, attackData.knockDuration);
                other.GetComponent<Hurtbox>().TakeDamage(attackData);
            }
        }
    }
}
