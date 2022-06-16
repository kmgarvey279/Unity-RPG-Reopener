using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class SliderBar : MonoBehaviour
{
    [SerializeField] private Slider slider;

    public void SetMaxValue(float maxValue)
    {
        slider.maxValue = maxValue;
    }

    public void SetCurrentValue(float currentValue)
    {
        slider.value = currentValue;      
    }

    public float GetCurrentValue()
    {
        return slider.value;
    }

    public float GetMaxValue()
    {
        return slider.maxValue;
    }
}
