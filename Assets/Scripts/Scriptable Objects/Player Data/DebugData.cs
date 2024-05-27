using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu(fileName = "DebugData", menuName = "Debug Data")]
public class DebugData : ScriptableObject
{
    [field: SerializeField] public LoadedData Data { private set; get; }
}
