using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;

public class BattleLog : MonoBehaviour
{
    [SerializeField] private GameObject display;
    [Header("Text")]
    [SerializeField] private TextMeshProUGUI textMeshProUGUI;

    public void ToggleDisplay(bool isDisplayed)
    {
        display.SetActive(isDisplayed);
    }

    public void UpdateText(string newText)
    {
        textMeshProUGUI.text = newText;
    }
}
