using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.EventSystems;

public class TargetIcon : MonoBehaviour, ISelectHandler, IDeselectHandler, IPointerEnterHandler
{
    public Button button;
    public Image image;
    public SignalSender onTargetChange;


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
        image.enabled = true;
        onTargetChange.Raise();
    }

    public void OnDeselect(BaseEventData eventData)
    {
        image.enabled = false;
    }

    public void OnPointerEnter(PointerEventData eventData)
    {
        Debug.Log("Enter");
            // EventSystem.current.SetSelectedGameObject(null);
            // EventSystem.current.SetSelectedGameObject(this.gameObject);
    }
}
