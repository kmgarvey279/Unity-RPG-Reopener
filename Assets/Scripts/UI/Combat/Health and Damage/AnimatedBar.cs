using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class AnimatedBar : MonoBehaviour
{
    [SerializeField] private SliderBar valueBar;
    [SerializeField] private SliderBar addBar;
    [SerializeField] private SliderBar subtractBar;
    [SerializeField] private SliderBar reverseBar;
    private float barTickSpeed = 0.25f;

    public void SetInitialValue(int maxValue, int currentValue)
    {
        valueBar.SetMaxValue(maxValue);
        valueBar.SetCurrentValue(currentValue);
        addBar.SetMaxValue(maxValue);
        subtractBar.SetMaxValue(maxValue);
        subtractBar.SetCurrentValue(currentValue);
        reverseBar.SetMaxValue(maxValue);
    }

    private void Update()
    {
        //if value bar is less than add bar, increase until it matches
        if(valueBar.GetCurrentValue() < addBar.GetCurrentValue())
        {
            StartCoroutine(TickUpCo());
        }
        //if subtract bar is greater than value bar, decrease until it matches
        else if(subtractBar.GetCurrentValue() > valueBar.GetCurrentValue())
        {
            StartCoroutine(TickDownCo());
        }
    }

    public void UpdateBar(int newValue)
    {
        //if new value is greater than current value
        if(newValue > valueBar.GetCurrentValue())
        {
            addBar.SetCurrentValue(newValue);
        }
        //if new value is less than current value
        else 
        {
            valueBar.SetCurrentValue(newValue);
        }
    }

    private IEnumerator TickUpCo()
    {
        yield return new WaitForSeconds(barTickSpeed);
        int newValue = valueBar.GetCurrentValue() + 1;
        valueBar.SetCurrentValue(newValue);
        subtractBar.SetCurrentValue(newValue);
        if(newValue == addBar.GetCurrentValue())
        {
            addBar.SetCurrentValue(0);
        }
    }

    private IEnumerator TickDownCo()
    {
        yield return new WaitForSeconds(barTickSpeed);
        subtractBar.SetCurrentValue(subtractBar.GetCurrentValue() - 1);
    }
}
