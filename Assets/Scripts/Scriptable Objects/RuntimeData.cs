using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu(fileName = "New Runtime Data", menuName = "Runtime Data")]
public class RuntimeData: ScriptableObject
{
    public bool lockInput;
    public bool interactTriggerCooldown;

    public string previousConnectorName;

    public void OnEnable()
    {
        previousConnectorName = "";
        lockInput = false;
        interactTriggerCooldown = false;
    }
}
