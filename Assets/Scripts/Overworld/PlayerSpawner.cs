using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerSpawner : MonoBehaviour
{
    [SerializeField] private GameObject playerPrefab;
    [SerializeField] private Transform spawnPoint;
    [SerializeField] private RuntimeData runtimeData;
    [SerializeField] private SceneConnector[] sceneConnectors;

    private void Start()
    {
        sceneConnectors = FindObjectsOfType(typeof(SceneConnector)) as SceneConnector[];
        SpawnPlayer();
    }

    public void SpawnPlayer()
    {
        if(runtimeData.previousConnectorName != "" && sceneConnectors.Length > 0)
        {
            for(int i = 0; i< sceneConnectors.Length; i++)
            {
                if(sceneConnectors[i].connectorName == runtimeData.previousConnectorName)
                {
                    GameObject playerObject = Instantiate(playerPrefab, sceneConnectors[i].spawnPoint.position, Quaternion.identity);
                    playerObject.transform.parent = gameObject.transform;        
                    Vector2 direction = (sceneConnectors[i].transform.position - sceneConnectors[i].spawnPoint.position).normalized;
                    playerObject.GetComponent<Player>().SetDirection(direction);        
                }
            }
        }
        else if(spawnPoint != null)
        {
            GameObject playerObject = Instantiate(playerPrefab, spawnPoint.position, Quaternion.identity);
            playerObject.transform.parent = gameObject.transform;  
        }
        else
        {
            Debug.Log("Nowhere to spawn player!");
        }
    }
}
