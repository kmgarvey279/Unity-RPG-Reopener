using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu]
[System.Serializable]
public class EnemyStats : CharacterStats
{
    [Header("Range")]
    public float chaseRadius;
    public float attackRadius;
    public float personalSpaceRadius;
}

