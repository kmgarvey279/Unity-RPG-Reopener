using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class SceneConnector : MonoBehaviour
{
    [field: Header("This Connector")]
    [field: SerializeField] public Vector2 Direction { get; private set; } 
    [field: SerializeField] public Transform SpawnPoint { get; private set; }

    [field: Header("Destination")]
    [field: SerializeField] public SceneField LinkedScene { get; private set; }
    [field: SerializeField] public int LinkedConnectorIndex { get; private set; } = 0;

    //[Header("Events")]
    //[SerializeField] private SignalSenderGO onEnterSceneConnector;




    public void OnTriggerEnter2D(Collider2D other)
    {
        if (other.CompareTag("PlayerChangeRoom"))
        {
            SceneSetupManager sceneSetupManager = FindObjectOfType<SceneSetupManager>();
            if (sceneSetupManager)
            {
                StartCoroutine(sceneSetupManager.OnExitSceneCo(LinkedScene.SceneName, LinkedConnectorIndex));
            }
        }
    }
}
