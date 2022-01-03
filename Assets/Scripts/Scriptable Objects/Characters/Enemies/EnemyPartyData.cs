using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu(fileName = "New Enemy Party Data", menuName = "EnemyPartyData")]
public class EnemyPartyData : ScriptableObject
{
    public List<GameObject> enemyPrefabs;
}
