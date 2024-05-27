using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;
using UnityEngine.UI;

public class StatPanel : MonoBehaviour
{
    [SerializeField] protected TextMeshProUGUI label;
    [SerializeField] protected TextMeshProUGUI statValue;
    [SerializeField] protected TextMeshProUGUI statCurrentValue;

    public void SetLabel(string _label)
    {
        label.text = _label;
    }

    public void SetValue(int value, bool isPercentage)
    {
        if (statValue != null)
        {
            string percentage = "";
            if (isPercentage)
            {
                percentage = "%";
            }
            statValue.text = value.ToString() + percentage;
        }
    }

    public void SetCurrentValue(int value)
    {
        if (statCurrentValue != null)
            statCurrentValue.text = value.ToString();
    }
}
