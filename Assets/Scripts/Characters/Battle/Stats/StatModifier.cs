using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[System.Serializable]
public class StatModifier
{
    [field: SerializeField] public StatType StatType { get; private set; }
    [field: SerializeField] public float Multiplier { get; private set; }
    [field: SerializeField] public float Additive { get; private set;  }
}
