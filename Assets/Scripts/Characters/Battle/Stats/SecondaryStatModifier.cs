using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[System.Serializable]
public class SecondaryStatModifier : MonoBehaviour
{
    [field: SerializeField] public SecondaryStatType SecondaryStatType { get; private set; }
    [field: SerializeField] public ModifierType ModifierType { get; private set; }
    [field: SerializeField] public float Modifier { get; private set; }
}
