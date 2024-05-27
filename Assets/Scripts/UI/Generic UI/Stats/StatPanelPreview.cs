using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;

public class StatPanelPreview : StatPanel
{
    [SerializeField] protected TextMeshProUGUI previewArrow;
    [SerializeField] protected Color decreaseIconColor;
    [SerializeField] protected Color increaseIconColor;

    [SerializeField] protected TextMeshProUGUI previewValue;
    [SerializeField] protected Color decreaseColor;
    [SerializeField] protected Color increaseColor;
    [SerializeField] protected Color neutralColor;

    public void DisplayPreview(int oldValue, int newValue, bool isPercentage)
    {
        previewArrow.color = neutralColor;
        previewValue.color = neutralColor;
        if (newValue > oldValue)
        {
            previewArrow.color = increaseIconColor;
            previewValue.color = increaseColor;
        }
        else if (newValue < oldValue)
        {
            previewArrow.color = decreaseIconColor;
            previewValue.color = decreaseColor;
        }

        string percentage = "";
        if (isPercentage)
        {
            percentage = "%";
        }
        previewValue.text = newValue.ToString() + percentage;
    }

    public void HidePreview()
    {
        previewArrow.color = neutralColor;
        previewValue.text = "";
    }

}
