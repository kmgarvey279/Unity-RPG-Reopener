using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using TMPro;

public class ActionPopup : MonoBehaviour
{
    [SerializeField] private GameObject display;
    [SerializeField] private Image icon;
    [SerializeField] private TextMeshProUGUI textUI;

    public void Awake()
    {
        Hide();
    }

    public void Display(string text, Sprite sprite)
    {
        textUI.text = text;
        icon.sprite = sprite;
        display.SetActive(true);
    }

    public void Hide()
    {
        display.SetActive(false);
        textUI.text = "";
        icon.sprite = null;
    }

}
