using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.EventSystems;

public class ControllableSlider : MonoBehaviour, ISelectHandler, IDeselectHandler
{
    [SerializeField] private SliderBar sliderBar;
    private float step = 0.001f;
    private bool isActive = false;
    
    public void Update()
    {
        float inputX = Input.GetAxisRaw("Horizontal");
        if (inputX > 0)
        {
            sliderBar.SetCurrentValue(sliderBar.GetCurrentValue() + step);
        }
        else if (inputX < 0)
        {
            sliderBar.SetCurrentValue(sliderBar.GetCurrentValue() - step);
        }
    }

    public void OnSelect(BaseEventData eventData)
    {
        isActive = true;
    }

    public void OnDeselect(BaseEventData eventData)
    {
        isActive = false;
    }
}
