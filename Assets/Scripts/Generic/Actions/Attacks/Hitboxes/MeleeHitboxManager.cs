using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using DG.Tweening;

public class MeleeHitboxManager : HitboxManager
{
    private CharacterStats characterStats;

    private void Start()
    {
        characterStats = GetComponentInParent<CharacterStats>();
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
}