using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[System.Serializable]
public class ClampInt
{
    [field: SerializeField] public int CurrentValue { get; private set; }
    [field: SerializeField] public int MinValue { get; private set; }
    [field: SerializeField] public int MaxValue { get; private set; }

    public ClampInt(int _value, int _minValue, int _maxValue)
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

    public void UpdateValue(int newValue)
    {
        CurrentValue = Mathf.Clamp(newValue, MinValue, MaxValue);
    }
}
