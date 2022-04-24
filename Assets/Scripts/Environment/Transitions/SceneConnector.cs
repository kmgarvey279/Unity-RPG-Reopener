using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class SceneConnector : Connector
{
    [Header("Scene Info")]
    [SerializeField] private string connectingSceneName;
    public string connectorName;

    public override IEnumerator ResolveEnterCo(GameObject player)
    {
        runtimeData.lockInput = true;
        onScreenFadeOut.Raise();
        runtimeData.previousConnectorName = connectorName;
        SceneManager.LoadScene(connectingSceneName);
        yield return new WaitForSeconds(1f); 
        Scene scene = SceneManager.GetSceneByName(connectingSceneName);
        SceneManager.SetActiveScene(scene);
    }
}
