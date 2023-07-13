using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ClampInt
{
    public int Value { get; private set; }
    public int MinValue { get; private set; }
    public int MaxValue { get; private set; }

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
        Value = Mathf.Clamp(_value, MinValue, MaxValue);
    }

    public void UpdateValue(int newValue)
    {
        Value = Mathf.Clamp(newValue, MinValue, MaxValue);
    }
}
