using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class SliderBar : MonoBehaviour
{
    [SerializeField] private Slider slider;

    public void SetMaxValue(int maxValue)
    {
        slider.maxValue = maxValue;
    }

    public void SetCurrentValue(int currentValue)
    {
        slider.value = currentValue;      
    }

    public int GetCurrentValue()
    {
        return (int)slider.value;
    }
}
