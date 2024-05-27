using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;
using UnityEngine.Events;
using UnityEngine.EventSystems;

public class SelectableButton : MonoBehaviour, ISelectHandler, IDeselectHandler
{
    [SerializeField] private GameObject cursor;
    [SerializeField] private GameObject highlight;
    [SerializeField] private GameObject highlightDisabled;
    [SerializeField] private OutlinedText text;
    [SerializeField] private Color disabledColor;
    [SerializeField] private SignalSenderGO onSelectButton;
    [SerializeField] private SignalSenderGO onClickButton;
    private bool isEnabled = true;

    public void ToggleEnabled(bool _isEnabled)
    {
        isEnabled = _isEnabled;
        if (isEnabled) 
        {
            text.SetTextColor(Color.white);
        }
        else 
        {
            text.SetTextColor(disabledColor);
        }
    }

    public void OnSelect(BaseEventData eventData)
    {
        Debug.Log("on select: " + this.name);
        if (isEnabled)
        {
            highlight.SetActive(true);
            highlightDisabled.SetActive(false);
        }
        else
        {
            highlight.SetActive(false);
            highlightDisabled.SetActive(true);
        }
        cursor.SetActive(true);
        if (onSelectButton)
        {
            onSelectButton.Raise(gameObject);
        }
    }

    public void OnDeselect(BaseEventData eventData)
    {
        Debug.Log("on deselect");
        highlight.SetActive(false);
        highlightDisabled.SetActive(false);

        cursor.SetActive(false);
    }

    public void OnClick()
    {
        Debug.Log("Clicked " + this.gameObject.name);
        if (isEnabled && onClickButton)
        {
            onClickButton.Raise(gameObject);
        }
    }
}
