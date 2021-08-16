using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.EventSystems;

public class TargetIcon : MonoBehaviour, ISelectHandler, IDeselectHandler, IPointerEnterHandler
{
    private Combatant combatant;
    [Header("Components")]
    [SerializeField] private Button button;
    [SerializeField] private Image image;
    [Header("Sprite Mask")]
    [SerializeField] private MaskController maskController;
    [Header("Events")]
    [SerializeField] private SignalSenderGO onTargetSelect;
    [SerializeField] private SignalSenderGO onTargetDeselect;

    private void Start()
    {
        combatant = GetComponentInParent<Combatant>();    
    }

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
        Select();
    }

    public void OnDeselect(BaseEventData eventData)
    {
        Deselect();
    }

    public void OnPointerEnter(PointerEventData eventData)
    {
        if(button.enabled)
        {
            Select();
        }
    }

    // public void OnPointerExit(PointerEventData eventData)
    // {
    //     if(button.enabled)
    //     {
    //         Deselect();
    //     }
    // }

    private void Select()
    {
        image.enabled = true;
        maskController.TriggerSelected();
        onTargetSelect.Raise(combatant.gameObject);
    }

    private void Deselect()
    {
        image.enabled = false;
        maskController.EndAnimation();
        onTargetDeselect.Raise(combatant.gameObject);
    }
}
