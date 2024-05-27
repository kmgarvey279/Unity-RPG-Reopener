using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public enum ElementalProperty
{
    Slash,
    Strike,
    Pierce,
    Fire,
    Ice,
    Electric,
    Dark,
    None
}

[CreateAssetMenu(fileName = "New Action", menuName = "Action/Attack")]
public class Attack : Action
{
    [field: Header("Health Effects")]
    [field: SerializeField] public float Power { get; protected set; } = 1f;
    [field: SerializeField] public int BreakBonus { get; protected set; }
    [field: Header("Damage Properties")]
    //[field: SerializeField] public IntStatType ActorStat { get; private set; }
    [field: SerializeField] public ElementalProperty ElementalProperty { get; private set; }
    [field: Header("Accuracy/Crit/Hits")]
    [field: SerializeField, Range(25, 100)] public int HitRate { get; protected set; } = 100;
    [field: SerializeField] public bool GuaranteedHit { get; protected set; } = false;
    [field: SerializeField, Range(0.25f, 2f)] public float CritRateMultiplier { get; protected set; } = 1f;
}
