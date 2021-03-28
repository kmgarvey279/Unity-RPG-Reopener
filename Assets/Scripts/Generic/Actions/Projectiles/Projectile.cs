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
    public Vector3 direction;
    [Header("Lifetime")]
    public float lifetime;
    [HideInInspector] public float lifetimeCounter;
    [SerializeField] private HitboxSO hitboxSO;
    private float damage;
    private string damageTag;

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

    public void SetDamage(CharacterInfo characterInfo)
    {
        if(hitboxSO.isSpecial)
        {
            damage = characterInfo.special.GetValue() * hitboxSO.damageMultiplier;
        }
        else
        {
            damage = characterInfo.attack.GetValue() * hitboxSO.damageMultiplier;
        }
    }

    public void Launch(Vector3 _direction)
    {
        direction = _direction;
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
        if(other.gameObject.CompareTag(damageTag) && other.isTrigger)
        {
            Rigidbody2D hitObject = other.GetComponentInParent<Rigidbody2D>();
            if(hitObject != null)
            {
                hitObject.DOMove((Vector3)hitObject.transform.position + direction * hitboxSO.knockForce, hitboxSO.knockDuration);
                other.GetComponent<Hurtbox>().TakeDamage(damage, hitboxSO);
            }
        }
    }
}
