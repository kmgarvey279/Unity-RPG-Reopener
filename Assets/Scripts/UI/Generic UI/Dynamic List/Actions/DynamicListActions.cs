using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.EventSystems;

public class DynamicListActions : DynamicList
{
    [Header("Data")]
    protected List<Action> dataList = new List<Action>();

    public void CreateList(List<Action> actions)
    {
        //set values
        visibleDataRange = new Vector2Int(0, slotLocations.Count - 3);
        this.dataList.AddRange(actions);
        this.dataListCount = dataList.Count;
        //create slots (hidden empty #0, visible range, hidden final if > visible range)
        for (int i = 0; i < slotLocations.Count; i++)
        {
            if (i > dataList.Count)
            {
                break;
            }
            //create slot
            GameObject slotObject = Instantiate(slotPrefab, Vector3.zero, Quaternion.identity);
            slotObject.transform.SetParent(slotListParent.transform, false);
            slotObject.GetComponent<RectTransform>().anchoredPosition = slotLocations[i].localPosition;
            DynamicListSlot dynamicListSlot = slotObject.GetComponent<DynamicListSlot>();
            dynamicListSlot.dynamicList = this;
            //assign values (ignore empty #0)
            if (i != 0)
            {
                AssignSlotData(dynamicListSlot, i - 1);
            }
            //deactivate first and last button
            if (i == 0 || i == slotLocations.Count - 1)
            {
                dynamicListSlot.button.interactable = false;
            }
            //add to list
            slotList.Add(dynamicListSlot);
        }
        //if there is at least one slot (aside from empty #0), select it
        if (slotList.Count > 1)
        {
            EventSystem.current.SetSelectedGameObject(null);
            EventSystem.current.SetSelectedGameObject(slotList[1].gameObject);
        }
    }

    public override void AssignSlotData(DynamicListSlot dynamicListSlot, int dataIndex)
    {
        DynamicListSlotAction dynamicListSlotAction = (DynamicListSlotAction)dynamicListSlot;
        if (dynamicListSlotAction)
        {
            dynamicListSlotAction.AssignAction(dataList[dataIndex]);
        }
    }

    public override void ClearList()
    {
        base.ClearList();
        dataList.Clear();
    }
}
