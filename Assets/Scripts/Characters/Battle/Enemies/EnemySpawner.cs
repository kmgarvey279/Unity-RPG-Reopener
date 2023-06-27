using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class EnemySpawner : MonoBehaviour
{
    [Header("Dependencies")]
    [SerializeField] private Camera mainCamera;
    [SerializeField] private BattleManager battleManager;
    [Header("Prefabs")]
    [SerializeField] private GameObject enemyPrefab;
    [Header("Spawn Positions")]
    [SerializeField] private List<Tile> spawnPositions = new List<Tile>();

    public EnemyCombatant SpawnEnemy(EnemyInfo enemyInfo, int positionNum)
    {
        if(positionNum < spawnPositions.Count)
        {
            Tile tile = spawnPositions[positionNum];
            GameObject enemyObject = Instantiate(enemyPrefab, tile.transform.position, Quaternion.identity);
            enemyObject.transform.parent = transform;
            EnemyCombatant enemyCombatant = enemyObject.GetComponent<EnemyCombatant>();
            if (enemyCombatant)
            {
                //set data
                enemyCombatant.SetCharacterData(enemyInfo);
                //set tile
                tile.AssignOccupier(enemyCombatant);
                enemyCombatant.SetTile(tile);
                enemyCombatant.SetBattleManager(battleManager);
                //set camera for ui display
                enemyCombatant.SetUICamera(mainCamera);
                return enemyCombatant;
            }
            return enemyCombatant;
        }
        return null;
    }
}
