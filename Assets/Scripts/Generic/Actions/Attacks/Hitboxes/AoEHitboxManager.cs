using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using DG.Tweening;

public class AoEHitboxManager : HitboxManager
{
    private Animator myAnimator;
    private CharacterStats characterStats;

    private void Start()
    {
        myAnimator = GetComponent<Animator>();    
    }

    public void StartAttack(CharacterStats userStats)
    {
        characterStats = userStats;
        myAnimator.SetTrigger("Start");
    }

    public override float CalculateDamage()
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

    public void DestroyAttackPrefab()
    {
        Destroy(this.gameObject);
    }
}
