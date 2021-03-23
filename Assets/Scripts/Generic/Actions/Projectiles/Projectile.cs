using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using DG.Tweening;

public class Projectile : MonoBehaviour
{
    public Rigidbody2D projectileRB;
    public GameObject impactPrefab;
    [Header("Speed")]
    public float speed;
    [HideInInspector] public float attackPower;
    [Header("Lifetime")]
    public float lifetime;
    [HideInInspector] public float lifetimeCounter;
    [SerializeField] private AttackStats attack;
    private CharacterStats characterStats;
    public string otherTag;

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

    public void Launch(Vector2 velocity, CharacterStats userStats)
    {
        characterStats = userStats;
        projectileRB.velocity = velocity.normalized * speed;
        // transform.rotation = Quaternion.Euler(direction);
    }

    private void OnCollisionEnter2D(Collision2D other)
    {        
        Destroy(this.gameObject);
        Instantiate(impactPrefab, transform.position, Quaternion.identity);
    }

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

    private float CalculateDamage()
    {
        if(attack.isSpecial)
        {
            return characterStats.special.GetValue() * attack.damageMultiplier; 
        } 
        else
        {
            return characterStats.attack.GetValue() * attack.damageMultiplier;    
        }
    }
}
