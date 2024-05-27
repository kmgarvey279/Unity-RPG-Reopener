using System.Collections;
using System.Collections.Generic;
using UnityEngine;

//public enum ModifierType
//{
//    Addend,
//    Multiplier
//}

[System.Serializable]
public class IntStatModifier
{
    [field: SerializeField] public IntStatType IntStatType { get; private set; }
    //[field: SerializeField] public ModifierType ModifierType { get; private set; }
    [field: SerializeField] public int Modifier { get; private set; }
}
