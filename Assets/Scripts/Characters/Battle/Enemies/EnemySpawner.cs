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
            EnemyCombatant enemyCombatant = enemyObject.GetComponent<EnemyCombatant>();;
            //assign name + letter
            enemyObject.name = enemyCombatant.characterName + positionNum;
            //set data
            enemyCombatant.SetCharacterData(enemyCombatant.characterInfo);
            //set tile
            tile.AssignOccupier(enemyCombatant);
            enemyCombatant.tile = tile;
            enemyCombatant.preferredTile = tile;
            
            return enemyCombatant;
        }
        return null;
    }
}
