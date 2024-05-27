using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class AnimatedBar : MonoBehaviour
{
    [SerializeField] private GameObject display;
    [SerializeField] private SliderBar valueBar;
    [SerializeField] private SliderBar changeBar;
    private float barTickDuration = 0.25f;


    public void SetInitialValue(int maxValue, int currentValue)
    {
        valueBar.SetMaxValue(maxValue);
        valueBar.SetCurrentValue(currentValue);
        changeBar.SetMaxValue(maxValue);
        changeBar.SetCurrentValue(currentValue);
    }

    public void SetValue(int currentValue)
    {
        valueBar.SetCurrentValue(currentValue);
        changeBar.SetCurrentValue(currentValue);
    }

    public void DisplayChange(int newValue)
    {
        //display amount gained
        if (newValue > valueBar.GetCurrentValue())
        {
            StartCoroutine(BarTickCo(changeBar, newValue));
        }
        //display amount lost
        else 
        {
            StartCoroutine(BarTickCo(valueBar, newValue));
        }
    }

    public void ResolveChange(int newValue)
    {
        if (newValue > valueBar.GetCurrentValue())
        {
            StartCoroutine(BarTickCo(valueBar, newValue));
        }
        else if (newValue < changeBar.GetCurrentValue())
        {
            StartCoroutine(BarTickCo(changeBar, newValue));
        }
    }

    private IEnumerator BarTickCo(SliderBar barToChange, int end)
    {
        float timer = 0f;
        float start = barToChange.GetCurrentValue();

        while (timer < barTickDuration)
        {
            float newValue = Mathf.Lerp(start, end, timer / barTickDuration);
            barToChange.SetCurrentValue(newValue);

            timer += Time.deltaTime;

            yield return null;
        }
        barToChange.SetCurrentValue(end);
    }
}
