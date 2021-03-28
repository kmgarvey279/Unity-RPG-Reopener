using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu]
[System.Serializable]
public class HitboxSO : ScriptableObject
{
    [Header("Type")]
    public bool isSpecial;
    [Header("Damage")]
    public float baseDamage;
    public float damageMultiplier;
    [Header("Knockback")]
    public float knockForce;
    public float knockDuration;
    [Header("Stun")]
    public float stunDuration;
    [Header("Properties (Ex: fire)")]
    public AttackProperty[] attackProperties;
    //TODO other (ex: status effects)
}

