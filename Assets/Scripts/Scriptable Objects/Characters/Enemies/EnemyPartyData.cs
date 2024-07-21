using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu(fileName = "New Enemy Party Data", menuName = "EnemyPartyData")]
public class EnemyPartyData : ScriptableObject
{
    [field: SerializeField] public List<EnemyInfo> Enemies { get; private set; } = new List<EnemyInfo>(6);
    [field: SerializeField] public int EXP { get; private set; }
    [field: SerializeField] public AudioClip BGM { get; private set; }
    [field: SerializeField] public GameObject EnvironmentPrefab { get; private set; }
}
