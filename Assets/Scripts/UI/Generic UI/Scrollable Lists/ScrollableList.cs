using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.EventSystems;
using Pathfinding;
using System.Linq;
using Newtonsoft.Json.Linq;

public class ScrollableList : MonoBehaviour
{
    //protected int dataListCount;
    [Header("Slots")]
    [SerializeField] protected GameObject slotPrefab;
    [field: SerializeField] public List<ScrollableListSlot> SlotList { get; private set; } = new List<ScrollableListSlot>();
    [SerializeField] protected GameObject slotListParent;

    [Header("List Settings")]
    [SerializeField] protected bool selectFirstSlot;
    [SerializeField] protected int displayedSlotCount;
    [SerializeField] protected float slotHeight;
    [SerializeField] protected VerticalLayoutGroup layout;

    [Header("How big should the viewpoint be?")]
    [SerializeField] private float targetedHeight;

    [SerializeField] protected Vector2Int visibleDataRange = new Vector2Int(0, 0);
    [SerializeField] protected Vector2 currentRectPosition;

    [Header("Cycle Buttons")]
    [SerializeField] protected GameObject topButton;
    [SerializeField] protected GameObject bottomButton;

    public Scrollbar scrollbar;

    //if the list is already populated
    public void Awake()
    {
        visibleDataRange = new Vector2Int(0, displayedSlotCount - 1);

        //if the list is prepopulated
        SlotList = GetComponentsInChildren<ScrollableListSlot>().ToList();
        if (SlotList.Count > 0)
        {
            OnCreateList();
        }
    }

    //if the list will be dynamically populated
    public void CreateList(int listCount)
    {
        ClearList();

        //if (listCount == 1)
        //{
        //    topButton.SetActive(false);
        //    bottomButton.SetActive(false);
        //}
        //else
        //{
        //    topButton.SetActive(true);
        //    bottomButton.SetActive(true);
        //}

        //set values
        //visibleDataRange = new Vector2Int(0, displayedSlotCount - 1);

        //create slots 
        for (int i = 0; i < listCount; i++)
        {
            //create slot
            GameObject slotObject = Instantiate(slotPrefab, Vector3.zero, Quaternion.identity);
            slotObject.transform.SetParent(slotListParent.transform, false);

            ScrollableListSlot scrollableListSlot = slotObject.GetComponent<ScrollableListSlot>();

            //add to list
            SlotList.Add(scrollableListSlot);
        }

        OnCreateList();
        //if (SlotList.Count > 0)
        //{
        //    foreach (ScrollableListSlot slot in SlotList)
        //    {
        //        slot.SetParent(this);
        //    }

        //    RectTransform listRect = SlotList[0].GetComponent<RectTransform>();
        //    float height = listRect.rect.height;
        //    float spacing = layout.spacing;
        //    slotHeight = height + spacing;
        //}
    }

    private void OnCreateList()
    {
        if (SlotList.Count == 1)
        {
            topButton.SetActive(false);
            bottomButton.SetActive(false);
        }
        else
        {
            topButton.SetActive(true);
            bottomButton.SetActive(true);
        }

        if (SlotList.Count > 0)
        {
            foreach (ScrollableListSlot slot in SlotList)
            {
                slot.SetParent(this);
            }

            RectTransform slotRect = SlotList[0].GetComponent<RectTransform>();
            float height = slotRect.rect.height;
            float width = slotRect.rect.width;
            float spacing = layout.spacing;
            slotHeight = height + spacing;
            //RectTransform thisRect = GetComponent<RectTransform>();
            //thisRect.SetSizeWithCurrentAnchors(RectTransform.Axis.Vertical, targetedHeight);
        }
        targetedHeight = slotHeight * displayedSlotCount + layout.padding.top;

        scrollbar.size = (float)displayedSlotCount / (float)SlotList.Count;

        //if (SlotList.Count > 0 && selectFirstSlot)
        //{
        //    EventSystem.current.SetSelectedGameObject(null);
        //    EventSystem.current.SetSelectedGameObject(SlotList[0].gameObject);
        //    SlotList[0].OnSelect(null);
        //}
    }

    public virtual void ClearList()
    {
        for (int i = 0; i < SlotList.Count; i++)
        {
            Destroy(SlotList[i].gameObject);
        }
        SlotList.Clear();

        visibleDataRange = new Vector2Int(0, displayedSlotCount - 1);
        RectTransform listRect = slotListParent.GetComponent<RectTransform>();
        listRect.anchoredPosition = new Vector2(listRect.anchoredPosition.x, 0);
    }

    public void OnSelectSlot(ScrollableListSlot slot)
    {
        int slotIndex = SlotList.IndexOf(slot);

        //if the topmost visible slot is selected, move list down       
        if (slotIndex == visibleDataRange.x && slotIndex > 0)
        {
            ScrollUp(slot);
        }
        //if the bottommost visible slot is selected, move list up
        else if (slotIndex == visibleDataRange.y && slotIndex < SlotList.Count - 1)
        {
            ScrollDown(slot);
        }
    }

    private void ScrollUp(ScrollableListSlot slot)
    {
        visibleDataRange.x--;
        visibleDataRange.y--;

        float difference = slotHeight * visibleDataRange.x;

        StartCoroutine(MoveListCo(difference, slot));
    }

    private void ScrollDown(ScrollableListSlot slot)
    {
        visibleDataRange.x++;
        visibleDataRange.y++;

        float difference = slotHeight * visibleDataRange.x;

        StartCoroutine(MoveListCo(difference, slot));
    }

    public IEnumerator CycleToTop()
    {
        yield return new WaitForEndOfFrame();

        EventSystem.current.SetSelectedGameObject(null);
        EventSystem.current.SetSelectedGameObject(SlotList[0].gameObject);
        OnSelectSlot(SlotList[0]);

        visibleDataRange = new Vector2Int(0, displayedSlotCount - 1);

        scrollbar.value = 0;

        StartCoroutine(MoveListCo(0, SlotList[0]));
    }

    public IEnumerator CycleToBottom()
    {
        yield return new WaitForEndOfFrame();

        EventSystem.current.SetSelectedGameObject(null);
        EventSystem.current.SetSelectedGameObject(SlotList[SlotList.Count - 1].gameObject);
        OnSelectSlot(SlotList[SlotList.Count - 1]);

        if (SlotList.Count > displayedSlotCount)
        {
            visibleDataRange = new Vector2Int(SlotList.Count - displayedSlotCount, SlotList.Count - 1);

            float difference = slotHeight * (SlotList.Count - displayedSlotCount);

            StartCoroutine(MoveListCo(difference, SlotList[SlotList.Count - 1]));
        }
    }

    private IEnumerator MoveListCo(float change, ScrollableListSlot currentSlot)
    {
        Debug.Log("Changing y value: " + change);
        float counter = 0f;
        float duration = 0.1f;
        RectTransform listRect = slotListParent.GetComponent<RectTransform>();

        Vector2 start = listRect.anchoredPosition;

        currentRectPosition = new Vector2(listRect.anchoredPosition.x, change);

        while (counter < duration)
        {
            listRect.anchoredPosition = Vector3.Lerp(start, currentRectPosition, (counter / duration));
            counter += Time.unscaledDeltaTime;
            yield return null;
        }
        listRect.anchoredPosition = currentRectPosition;
        yield return null;

        //if (visibleDataRange.x == 0)
        //{
        //    topButton.SetActive(true);
        //}
        //else
        //{
        //    topButton.SetActive(false);
        //}

        //if (visibleDataRange.y == SlotList.Count - 1)
        //{
        //    bottomButton.SetActive(true);
        //}
        //else
        //{
        //    Button.SetActive(false);
        //}

        scrollbar.value = currentRectPosition.y / (slotHeight * (float)(SlotList.Count - displayedSlotCount));

    }
}
