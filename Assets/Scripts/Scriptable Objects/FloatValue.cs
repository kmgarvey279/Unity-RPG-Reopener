using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu]
[System.Serializable]
public class FloatValue : ScriptableObject
{
    public float defaultValue;
    
    public float runtimeValue;

    private void OnEnable()
    {
        runtimeValue = defaultValue;
    }
}
