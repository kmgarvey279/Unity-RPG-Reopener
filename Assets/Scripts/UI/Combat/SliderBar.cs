using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class SliderBar : MonoBehaviour
{
    [SerializeField] private Slider slider;

    public void SetSliderValues(float maxValue, float startValue)
    {
        slider.maxValue = maxValue;
        slider.value = maxValue;
    }

    public void UpdateSlider(float newValue)
    {
        slider.value = newValue;      
    }
}
