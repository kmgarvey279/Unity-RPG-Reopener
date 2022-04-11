using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class EnemySpawner : MonoBehaviour
{
    [Header("Spawn Positions")]
    public List<Tile> spawnPositions = new List<Tile>();

    public Combatant SpawnEnemy(GameObject enemyPrefab, int positionNum)
    {
        if(positionNum <= spawnPositions.Count)
        {
            Tile tile = spawnPositions[positionNum - 1];
            GameObject enemyObject = Instantiate(enemyPrefab, tile.transform.position, Quaternion.identity);
            enemyObject.transform.parent = gameObject.transform;

            Combatant combatant = enemyObject.GetComponent<Combatant>();
            enemyObject.name = combatant.characterName + positionNum;
            tile.AssignOccupier(combatant);
            combatant.tile = tile;
            return combatant;
        }
        return null;
    }
}
