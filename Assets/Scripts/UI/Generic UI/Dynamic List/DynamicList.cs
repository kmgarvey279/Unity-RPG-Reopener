using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.EventSystems;

public class DynamicList : MonoBehaviour
{
    protected int dataListCount;
    [Header("Slot Object")]
    [SerializeField] protected GameObject slotPrefab;
    [Header("List")]
    protected List<DynamicListSlot> slotList = new List<DynamicListSlot>();
    [SerializeField] protected GameObject slotListParent;
    [SerializeField] protected List<RectTransform> slotLocations = new List<RectTransform>();
    protected Vector2Int visibleDataRange = new Vector2Int(0, 0);

    public virtual void AssignSlotData(DynamicListSlot dynamicListSlot, int dataIndex)
    {
    }

    public virtual void ClearList()
    {
        for (int i = 0; i < slotList.Count; i++)
        {
            Destroy(slotList[i].gameObject);
        }
        slotList.Clear();
    }

    public void OnSelectSlot(DynamicListSlot dynamicListSlot)
    {
        int slotIndex = slotList.IndexOf(dynamicListSlot);
        //if the topmost visible slot is selected, move list down and move the bottommost hidden item to top
        if (slotIndex == 1 && visibleDataRange.x > 0)
        {
            ScrollUp();
        }
        //if the bottommost visible slot is selected
        if (slotIndex == slotList.Count - 2 && visibleDataRange.y < dataListCount - 1)
        {
            ScrollDown();
        }
    }

    public virtual void OnClickSlot(DynamicListSlot dynamicListSlot)
    {
    }

    private void ScrollUp()
    {
        visibleDataRange.x--;
        visibleDataRange.y--;

        //deactivate visible bottom visible button
        slotList[slotList.Count - 2].button.interactable = false;
        //move invisible bottom button to the top
        DynamicListSlot slotToMove = slotList[slotList.Count - 1];
        slotList.Remove(slotToMove);
        slotList.Insert(0, slotToMove);
        if (visibleDataRange.x - 1 >= 0)
        {
            AssignSlotData(slotToMove, visibleDataRange.x - 1);
        }
        slotToMove.GetComponent<RectTransform>().anchoredPosition = slotLocations[0].localPosition;
        //shift all other panels down
        for (int i = 1; i < slotList.Count; i++)
        {
            RectTransform slotRect = slotList[i].GetComponent<RectTransform>();
            if (slotRect.anchoredPosition != (Vector2)slotLocations[i].localPosition)
            {
                Button buttonToActivate = slotList[1].button;
                StartCoroutine(MoveSlotCo(slotList[i], slotLocations[i].localPosition, buttonToActivate));
            }
        }
    }

    private void ScrollDown()
    {
        visibleDataRange.x++;
        visibleDataRange.y++;

        slotList[1].button.interactable = false;
        //move topmost (offscreen) panel to the bottom (offscreen)
        DynamicListSlot slotToMove = slotList[0];
        slotList.Remove(slotToMove);
        slotList.Add(slotToMove);
        if (visibleDataRange.y + 1 <= dataListCount - 1)
        {
            AssignSlotData(slotToMove, visibleDataRange.y + 1);
        }
        slotToMove.GetComponent<RectTransform>().anchoredPosition = slotLocations[slotList.Count - 1].localPosition;
        //shift all other panels up
        for (int i = slotList.Count - 1; i >= 0; i--)
        {
            RectTransform slotRect = slotList[i].GetComponent<RectTransform>();
            if (slotRect.anchoredPosition != (Vector2)slotLocations[i].localPosition)
            {
                Button buttonToActivate = slotList[slotList.Count - 2].button;
                StartCoroutine(MoveSlotCo(slotList[i], slotLocations[i].localPosition, buttonToActivate));
            }
        }
    }

    private IEnumerator MoveSlotCo(DynamicListSlot slot, Vector2 newPosition, Button buttonToActivate)
    {

        float counter = 0f;
        float duration = 0.15f;
        RectTransform slotRect = slot.GetComponent<RectTransform>();
        Vector2 start = slotRect.anchoredPosition;

        while (counter < duration)
        {
            slotRect.anchoredPosition = Vector3.Lerp(start, newPosition, (counter / duration));
            counter += Time.deltaTime;
            yield return null;
        }
        slotRect.anchoredPosition = newPosition;
        buttonToActivate.interactable = true;
        yield return null;
    }
}
