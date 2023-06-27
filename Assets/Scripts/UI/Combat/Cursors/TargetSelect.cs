using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.EventSystems;

public class TargetSelect : MonoBehaviour, ISelectHandler, IDeselectHandler
{
    [SerializeField] private Canvas canvas;
    private Combatant combatant;
    [SerializeField] private Image cursor;
    [SerializeField] private SignalSenderGO onCombatantSelect;
    [SerializeField] private SignalSenderGO onCombatantDeselect;

    private void Start()
    {
        combatant = GetComponentInParent<Combatant>();    
    }

    public void SetUICamera(Camera camera)
    {
        canvas.worldCamera = camera;
        canvas.overrideSorting = true;
    }

    public void ToggleButton(bool enable)
    {
        GetComponent<Button>().enabled = enable;
    }

    public void OnSelect(BaseEventData eventData)
    {
        Select();
    }

    public void Select()
    {
        cursor.enabled = true;
        onCombatantSelect.Raise(combatant.gameObject);
    }

    public void OnDeselect(BaseEventData eventData)
    {
        Deselect();
    }

    public void Deselect()
    {
        cursor.enabled = false;
        onCombatantDeselect.Raise(combatant.gameObject);
    }
}
