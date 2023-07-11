using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class InterventionPoints : MonoBehaviour
{
    [field: SerializeField] public int Value { get; private set; } = 0;
    [SerializeField] private AnimatedBar interventionBar;
    [SerializeField] private SliderBar glowBar;
    [SerializeField] private OutlinedText amountNum;


    private WaitForSeconds waitShort = new WaitForSeconds(0.2f);

    public void UpdateValue(int change)
    {
        Value = Mathf.Clamp(Value + change, 0, 100);
        StartCoroutine(DisplayNewValue());
    }

    public IEnumerator DisplayNewValue()
    {
        interventionBar.DisplayChange(Value);
        if (Value < 25)
        {
            glowBar.SetCurrentValue(0);
        }
        else
        {
            glowBar.SetCurrentValue(Value);
        }
        amountNum.SetText(Value.ToString());
        yield return waitShort;

        interventionBar.ResolveChange(Value);
    }
}
