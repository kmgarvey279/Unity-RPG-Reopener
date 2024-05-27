using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;

public class TextBox : MonoBehaviour
{
    [SerializeField] private TextMeshProUGUI primaryTMP;
    [SerializeField] private TextMeshProUGUI primaryTMPBorder;
    [SerializeField] private TextMeshProUGUI secondaryTMP;
    [SerializeField] private TextMeshProUGUI secondaryTMPBorder;

    public void SetText(string text, string secondaryText = "")
    {
        primaryTMP.text = text;
        primaryTMPBorder.text = text;

        secondaryTMP.text = secondaryText;
        secondaryTMPBorder.text = secondaryText;
    }

    public void Clear()
    {
        primaryTMP.text = "";
        primaryTMPBorder.text = "";

        secondaryTMP.text = "";
        secondaryTMPBorder.text = "";
    }
}
