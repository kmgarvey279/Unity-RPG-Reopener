using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[System.Serializable]
public class AttackData
{
    [Header("Attack power")]
    public float attackPower;
    public float attackMultiplier;
    [Header("Direction")]
    public Vector3 direction = new Vector3(0,0);
    [Header("Knockback")]
    public float knockDuration;
    public float knockForce;
    [Header("Type of ability")]
    public bool isHeal;
    public bool isSpecial;
    [Header("Properties")]
    public AttackProperty[] properties;
    public StatusEffect[] statusEffects;
    [Header("Hurt player or enemy?")]
    public string damageTag;
}
