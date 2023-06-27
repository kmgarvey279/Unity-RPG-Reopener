using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class InterventionPointsDisplay : MonoBehaviour
{
    [SerializeField] private AnimatedBar interventionBar;
    [SerializeField] private SliderBar glowBar;
    [SerializeField] private OutlinedText amountNum;


    private WaitForSeconds waitShort = new WaitForSeconds(0.25f);

    public IEnumerator SpendPoints(int newValue)
    {
        interventionBar.DisplayChange(newValue);
        glowBar.SetCurrentValue(newValue);
        if(newValue < 25)
        {
            glowBar.SetCurrentValue(0);
        }
        amountNum.SetText(newValue.ToString());
        yield return waitShort;

        interventionBar.ResolveChange(newValue);
    }

    public IEnumerator GainPoints(int newValue)
    {
        interventionBar.DisplayChange(newValue);
        if(newValue >= 25)
        {
            glowBar.SetCurrentValue(newValue);
        }
        amountNum.SetText(newValue.ToString());
        yield return waitShort;

        interventionBar.ResolveChange(newValue);
    }
}
