using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;

public class ActionPopup : MonoBehaviour
{
    [SerializeField] private GameObject display;
    [SerializeField] private TextMeshProUGUI textUI;
    
    public void Display(string actionName)
    {
        textUI.text = actionName;
        display.SetActive(true);
    }

    public void Hide()
    {
        display.SetActive(false);
        textUI.text = "";
    }

}
