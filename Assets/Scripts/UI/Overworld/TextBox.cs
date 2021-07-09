using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using TMPro;

public class TextBox : MonoBehaviour
{
    [SerializeField] private GameObject panel;
    [SerializeField] private TextMeshProUGUI textField;
    [SerializeField] private Image portrait;

    public void DisplayTextBox(string text)
    {
        panel.SetActive(true);
        textField.text = text;
    }

    public void HideTextBox()
    {
        panel.SetActive(false);
        textField.text = "";
    }
}
