using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ClampValue : ScriptableObject
{
    [SerializeField] private float maxValue;
    public float currentValue;

    private void OnEnable()
    {
        currentValue = maxValue;
    }

    public void ChangeCurrentValue(float difference)
    {
        float temp = currentValue + difference;
        currentValue = Mathf.Clamp(temp, 0, maxValue);
    }
}
