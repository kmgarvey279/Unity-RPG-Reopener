using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class AnimatedBar : MonoBehaviour
{
    [SerializeField] private SliderBar valueBar;
    [SerializeField] private SliderBar addBar;
    [SerializeField] private SliderBar subtractBar;
    // [SerializeField] private SliderBar reverseBar;
    private float barTickSpeed = 0.001f;

    public void SetInitialValue(float maxValue, float currentValue)
    {
        valueBar.SetMaxValue((int)maxValue);
        valueBar.SetCurrentValue((int)currentValue);
        addBar.SetMaxValue((int)maxValue);
        subtractBar.SetMaxValue((int)maxValue);
        subtractBar.SetCurrentValue((int)currentValue);
        // reverseBar.SetMaxValue((int)maxValue);
    }

    // private void Update()
    // {
    //     //if value bar is less than add bar, increase until it matches
    //     if(valueBar.GetCurrentValue() < addBar.GetCurrentValue())
    //     {
    //         StartCoroutine(TickUpCo());
    //     }
    //     //if subtract bar is greater than value bar, decrease until it matches
    //     else if(subtractBar.GetCurrentValue() > valueBar.GetCurrentValue())
    //     {
    //         StartCoroutine(TickDownCo());
    //     }
    // }

    public void DisplayChange(float newValue)
    {
        //display amount gained
        if(newValue > valueBar.GetCurrentValue())
        {
            addBar.SetCurrentValue(newValue);
        }
        //display amount lost
        else 
        {
            valueBar.SetCurrentValue(newValue);
            addBar.SetCurrentValue(newValue);
        }
    }

    public void ResolveChange()
    {
        if(valueBar.GetCurrentValue() < addBar.GetCurrentValue())
        {
            StartCoroutine(TickUpCo());
        }
        else if(subtractBar.GetCurrentValue() > valueBar.GetCurrentValue())
        {
            StartCoroutine(TickDownCo());
        }
    }

    private IEnumerator TickUpCo()
    {
        float tick = valueBar.GetMaxValue() / 100f;
        while(valueBar.GetCurrentValue() < addBar.GetCurrentValue())
        {
            yield return new WaitForSeconds(barTickSpeed);
            float newValue = valueBar.GetCurrentValue() + tick;
            if(newValue > addBar.GetCurrentValue())
            {
                newValue = addBar.GetCurrentValue();
            }
            valueBar.SetCurrentValue(newValue);
            subtractBar.SetCurrentValue(newValue);
        }
    }

    private IEnumerator TickDownCo()
    {
        float tick = valueBar.GetMaxValue() / 100f;
        while(subtractBar.GetCurrentValue() > valueBar.GetCurrentValue())
        {
            yield return new WaitForSeconds(barTickSpeed);
            float newValue = subtractBar.GetCurrentValue() - tick;
            if(newValue < valueBar.GetCurrentValue())
            {
                newValue = valueBar.GetCurrentValue();
            }
            subtractBar.SetCurrentValue(newValue);
        }
    }
}
