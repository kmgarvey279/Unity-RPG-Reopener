using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;

public class OutlinedText : MonoBehaviour
{
    [SerializeField] private TextMeshProUGUI mainText;
    [SerializeField] private List<TextMeshProUGUI> outline = new List<TextMeshProUGUI>();

    public void SetText(string newText)
    {
        mainText.text = newText;
        foreach(TextMeshProUGUI tmp in outline)
        {
            tmp.text = newText;
        }
    }

    public void SetFontSize(int newSize)
    {
        mainText.fontSize = newSize;
        foreach (TextMeshProUGUI tmp in outline)
        {
            tmp.fontSize = newSize;
        }
    }

    public void SetTextColor(Color newColor)
    {
        mainText.color = newColor;
    }

    public void SetSecondaryTextColor(Color newColor)
    {
        foreach (TextMeshProUGUI tmp in outline)
        {
            tmp.color = newColor;
        }
    }

    public void Clear()
    {
        mainText.text = "";
        foreach (TextMeshProUGUI tmp in outline)
        {
            tmp.text = "";
        }
    }
}
