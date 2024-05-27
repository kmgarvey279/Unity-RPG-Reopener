using System.Collections;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;
using UnityEngine.Diagnostics;

public class PlayerSpawner : MonoBehaviour
{
    [Header("Player Object")]
    [SerializeField] private GameObject playerPrefab;
    public Player Player { get; private set; }

    [SerializeField] private List<SceneConnector> sceneConnectors;

    public void Awake()
    {
        sceneConnectors = GetComponentsInChildren<SceneConnector>().ToList();
    }

    public void SpawnPlayer(int sceneConnectorIndex)
    {
        if (sceneConnectors.Count > sceneConnectorIndex)
        {
            GameObject playerObject = Instantiate(playerPrefab, sceneConnectors[sceneConnectorIndex].SpawnPoint.position, Quaternion.identity);
            if (playerObject != null)
            {
                Player = playerObject.GetComponent<Player>();
                playerObject.transform.SetParent(transform, true);
            }
            Player.SetDirection(sceneConnectors[sceneConnectorIndex].Direction);
        }
        else if (sceneConnectors.Count > 0)
        {
            Debug.Log("Error: Requested scene connector index not found, defaulting to index 0");
            GameObject playerObject = Instantiate(playerPrefab, sceneConnectors[0].SpawnPoint.position, Quaternion.identity);
            if (playerObject != null)
            {
                Player = playerObject.GetComponent<Player>();
                playerObject.transform.SetParent(transform, true);
            }
            Player.SetDirection(sceneConnectors[0].Direction);
        }
        else
        {
            Debug.Log("Error: No scene connectors have been set in this scene!");
        }
    }
}
