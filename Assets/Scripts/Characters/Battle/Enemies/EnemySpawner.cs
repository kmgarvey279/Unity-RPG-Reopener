using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class EnemySpawner : MonoBehaviour
{
    [Header("Spawn Positions")]
    public List<Transform> spawnPositions = new List<Transform>();

    public Combatant SpawnEnemy(GameObject EnemyPrefab, int positionNum)
    {
        if(positionNum <= spawnPositions.Count)
        {
            GameObject enemyObject = Instantiate(EnemyPrefab, spawnPositions[positionNum - 1].position, Quaternion.identity);
            enemyObject.transform.parent = gameObject.transform;
            return enemyObject.GetComponent<Combatant>();
        }
        return null;
    }
}
