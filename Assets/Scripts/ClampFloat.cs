using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ClampFloat
{
    public float CurrentValue { get; private set; }
    public float MinValue { get; private set; }
    public float MaxValue { get; private set; }

    public ClampFloat(float _value, float _minValue, float _maxValue)
    {
        if (_minValue > _maxValue)
        {
            _minValue = _maxValue;
        }
        if (_maxValue < _minValue)
        {
            _maxValue = _minValue;
        }
        MinValue = _minValue;
        MaxValue = _maxValue;
        CurrentValue = Mathf.Clamp(_value, MinValue, MaxValue);
    }

    public void UpdateValue(float newValue)
    {
        CurrentValue = Mathf.Clamp(newValue, MinValue, MaxValue);
    }
}