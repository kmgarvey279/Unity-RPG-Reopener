using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;
using UnityEngine.EventSystems;

public class DynamicListSlotItem : DynamicListSlot
{
    [SerializeField] private SignalSenderString onUpdateDescription;
    [SerializeField] private SignalSenderGO onSelectItemBattle;
    [Header("Item Info")]
    [SerializeField] private OutlinedText countText;
    public InventoryItem InventoryItem { get; private set; }

    public void AssignItem(InventoryItem inventoryItem)
    {
        this.InventoryItem = inventoryItem;
        nameText.SetText(inventoryItem.Item.ItemName);
        countText.SetText("x " + inventoryItem.Count.ToString("n0"));
    }

    public override void OnSelect(BaseEventData eventData)
    {
        base.OnSelect(eventData);
        onUpdateDescription.Raise(InventoryItem.Item.ItemDescription);
    }

    public void OnClick()
    {
        onSelectItemBattle.Raise(this.gameObject);
    }
}
