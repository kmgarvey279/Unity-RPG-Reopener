using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public enum UniversalModifierType
{
    Damage,
    Healing,
    MPRegen
}

[System.Serializable]
public class UniversalModifier
{
    [field: Header("Event Type")]
    [field: SerializeField] public BattleEventType BattleEventType { get; private set; }
    [field: Header("Modifier")]
    [field: SerializeField] public UniversalModifierType UniversalModifierType { get; private set; }
    [field: SerializeField] public float ModifierValue { get; private set; }
}
