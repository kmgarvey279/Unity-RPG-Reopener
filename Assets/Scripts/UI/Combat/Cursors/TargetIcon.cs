using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.EventSystems;

public class TargetIcon : MonoBehaviour, ISelectHandler, IDeselectHandler
{
    public Button button;
    public Image image;
    // public SignalSenderGO changeCameraFocus;

    public void ToggleButton(bool isEnabled)
    {
        button.enabled = isEnabled;
    }

    public void ToggleImage(bool isEnabled)
    {
        image.enabled = isEnabled;
    }

    public void OnSelect(BaseEventData eventData)
    {
        // changeCameraFocus.Raise(this.gameObject);
        image.enabled = true;
    }

    public void OnDeselect(BaseEventData eventData)
    {
        image.enabled = false;
    }

    public void OnClick()
    {
        image.enabled = false;
    }
}
