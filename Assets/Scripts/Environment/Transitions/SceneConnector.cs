using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class SceneConnector : Connector
{
    [Header("Scene Info")]
    [SerializeField] private string connectingSceneName;
    public string connectorName;
}
