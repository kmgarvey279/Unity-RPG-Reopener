using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using TMPro;

public class InventorySlot : MonoBehaviour
{
    [Header("Item In Slot")]
    [SerializeField] private ItemObject item;
    [Header("UI Display")]
    [SerializeField] private Image slotImage;
    [SerializeField] private TextMeshProUGUI amountText;
    [SerializeField] private TextMeshProUGUI nameText;
    [Header("UI Varables")]
    private bool isSelected = false;

    public void ToggleSelected(bool newBool)
    {
        isSelected = newBool;
    }

    public void AssignSlot(ItemObject newItem)
    {
        item = newItem;
        slotImage.sprite = item.icon;
        nameText.text = item.name;
        amountText.text = item.numHeld.ToString("n0");
    }

    public void ClearSlot()
    {
        Destroy(this.gameObject); 
    }
}
