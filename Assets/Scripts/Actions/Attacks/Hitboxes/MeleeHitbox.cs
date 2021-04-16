using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using DG.Tweening;
using System;

public class MeleeHitbox : Hitbox
{
    public Character character;

    private void OnEnable()
    {
        character = GetComponentInParent<Character>(); 
        
        if(attackData.isSpecial)
        {
            attackData.attackPower = character.characterInfo.special.GetValue();
        }
        else
        {
            attackData.attackPower = character.characterInfo.attack.GetValue();
        }
        attackData.direction = character.lookDirection;
    }
}
