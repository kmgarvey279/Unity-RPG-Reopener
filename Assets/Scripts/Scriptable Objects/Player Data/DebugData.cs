using System.Collections;
using System.Collections.Generic;
using System.Linq.Expressions;
using UnityEngine;

[CreateAssetMenu(fileName = "DebugData", menuName = "Debug Data")]
public class DebugData : ScriptableObject
{
    [field: SerializeField] public PlayerData Data { private set; get; }

}
