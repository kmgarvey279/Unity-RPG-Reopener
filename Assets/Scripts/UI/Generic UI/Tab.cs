using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;
using UnityEngine.UI;

public enum TabStateType
{
    Default,
    Selected,
    Disabled
}

public class Tab : MonoBehaviour
{
    [SerializeField] protected Image background;
    [SerializeField] protected TextMeshProUGUI label;

    [SerializeField] protected Color defaultColor;
    [SerializeField] protected Color selectedColor;
    [SerializeField] protected Color disabledColor;

    public void SetLabel(string text)
    {
        label.text = text;
    }

    public void ChangeState(TabStateType tabStateType) 
    { 
        if (tabStateType == TabStateType.Selected)
        {
            background.color = selectedColor;
        }
        else if (tabStateType == TabStateType.Disabled)
        {
            background.color = disabledColor;
        }
        else
        {
            background.color = defaultColor;
        }
    }
}
