using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu(fileName = "New Enemy Party Data", menuName = "EnemyPartyData")]
public class EnemyPartyData : ScriptableObject
{
    [field: SerializeField] public List<EnemyInfo> Enemies { get; private set; } = new List<EnemyInfo>(6);
}
