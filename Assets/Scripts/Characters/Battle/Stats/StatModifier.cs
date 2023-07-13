using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public enum ModifierType
{
    Addend,
    Multiplier
}

[System.Serializable]
public class StatModifier
{
    [field: SerializeField] public StatType StatType { get; private set; }
    [field: SerializeField] public ModifierType ModifierType { get; private set; }
    [field: SerializeField] public float Modifier { get; private set; }
}
