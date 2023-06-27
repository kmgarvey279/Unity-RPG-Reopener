using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu(fileName = "New Overworld Data", menuName = "Runtime Data/Overworld Data")]
public class OverworldData: ScriptableObject
{
    public bool lockInput;
    public bool interactTriggerCooldown;

    [Header("Connector Info")]
    public string previousConnectorName;

    [Header("Scene Info")]
    public string currentSceneName;
    public Vector2 playerLocation;

    public void OnEnable()
    {
        previousConnectorName = "";
        
        currentSceneName = "";
        playerLocation = Vector2.zero;

        lockInput = false;
        interactTriggerCooldown = false;
    }
}
