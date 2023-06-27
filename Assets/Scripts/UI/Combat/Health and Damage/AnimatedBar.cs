using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class AnimatedBar : MonoBehaviour
{
    private GameObject display;
    [SerializeField] private SliderBar valueBar;
    [SerializeField] private SliderBar changeBar;
    private float barTickDuration = 0.2f;
    private bool valueAnimationActive = false;
    private bool changeAnimationActive = false;
    private bool queueHide = false;

    public void ToggleDisplay(bool show)
    {
        if(!show && (changeAnimationActive || valueAnimationActive))
        {
            queueHide = true;
        }
        else
        {
            display.SetActive(show);
        }
    }

    public void SetInitialValue(float maxValue, float currentValue)
    {
        valueBar.SetMaxValue((int)maxValue);
        valueBar.SetCurrentValue((int)currentValue);
        changeBar.SetMaxValue((int)maxValue);
        changeBar.SetCurrentValue((int)currentValue);
    }

    public void DisplayChange(float newValue)
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

    public void ResolveChange(float newValue)
    {
        if(newValue > valueBar.GetCurrentValue())
        {
            StartCoroutine(BarTickCo(valueBar, newValue));
        }
        else if(newValue < changeBar.GetCurrentValue())
        {
            StartCoroutine(BarTickCo(changeBar, newValue));
        }
    }

    private IEnumerator BarTickCo(SliderBar barToChange, float end)
    {
        if (barToChange == valueBar)
        {
            valueAnimationActive = true;
        }
        else if (barToChange == changeBar)
        {
            changeAnimationActive = true;
        }

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

        if (barToChange == valueBar)
        {
            valueAnimationActive = false;
        }
        else if (barToChange == changeBar)
        {
            changeAnimationActive = false;
        }

        if (queueHide)
        {
            queueHide = false;
            display.SetActive(false);
        }
    }

    //private IEnumerator TickDownCo(SliderBar barToChange)
    //{
    //    animationActive = true;

    //    float timer = 0f;
    //    float start = changeBar.GetCurrentValue();
    //    float end = valueBar.GetCurrentValue();

    //    while (timer < barTickDuration)
    //    {
    //        float newValue = Mathf.Lerp(start, end, timer / barTickDuration);
    //        changeBar.SetCurrentValue(newValue);

    //        timer += Time.deltaTime;

    //        yield return null;
    //    }
    //    changeBar.SetCurrentValue(end);

    //    animationActive = false;
    //    if (queueHide)
    //    {
    //        queueHide = false;
    //        display.SetActive(false);
    //    }
    //}
}
