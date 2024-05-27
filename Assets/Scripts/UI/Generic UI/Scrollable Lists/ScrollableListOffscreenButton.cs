using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.EventSystems;

public class ScrollableListOffscreenButton : MonoBehaviour, ISelectHandler
{
    private enum OffscreenButtonType
    {
        Top,
        Bottom
    }
    [SerializeField] private OffscreenButtonType offscreenButtonType;
    [SerializeField] private ScrollableList parentList;

    private void Awake()
    {
        parentList = GetComponentInParent<ScrollableList>();
    }

    public void OnSelect(BaseEventData eventData)
    {
        if (offscreenButtonType == OffscreenButtonType.Top)
        {
            StartCoroutine(parentList.CycleToBottom());
        }
        else
        {
            StartCoroutine(parentList.CycleToTop());
        }
    }
}
