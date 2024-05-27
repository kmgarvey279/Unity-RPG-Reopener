using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.EventSystems;
using UnityEngine.UI;

public abstract class ScrollableListSlot : MonoBehaviour, ISelectHandler, IDeselectHandler
{
    [SerializeField] protected GameObject cursor;
    [SerializeField] protected ScrollableList parentList;
    protected Button button;

    protected virtual void Awake()
    {
        button = GetComponent<Button>();
    }

    public void SetParent(ScrollableList scrollableList)
    {
        parentList = scrollableList;
    }

    public virtual void OnSelect(BaseEventData eventData)
    {
        parentList.OnSelectSlot(this);
        cursor.SetActive(true);
    }

    public virtual void OnDeselect(BaseEventData eventData)
    {
        cursor.SetActive(false);
    }

    public virtual void OnClick()
    {
    }

    public void ToggleButton(bool isInteractable)
    {
        button.interactable = isInteractable;
    }

}
