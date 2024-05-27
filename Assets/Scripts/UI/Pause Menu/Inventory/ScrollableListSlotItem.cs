using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.EventSystems;
using System.IO;

public class ScrollableListSlotItem : ScrollableListSlot
{
    [field: SerializeField] public Item Item { get; private set; }

    [SerializeField] private TextMeshProUGUI nameValue;
    [SerializeField] private TextMeshProUGUI amountValue;
    [SerializeField] private Image icon;
    [SerializeField] private SignalSenderGO onSelectItem;
    [SerializeField] private SignalSenderGO onClickItem;

    public override void OnSelect(BaseEventData eventData)
    {
        base.OnSelect(eventData);
        onSelectItem.Raise(this.gameObject);
    }

    public override void OnClick()
    {
        onClickItem.Raise(this.gameObject);
    }

    public void AssignItem(Item item, int count)
    {
        Item = item;

        nameValue.text = item.ItemName;
        amountValue.text = "x" + count;
        icon.sprite = item.ItemIcon;
    }
}
