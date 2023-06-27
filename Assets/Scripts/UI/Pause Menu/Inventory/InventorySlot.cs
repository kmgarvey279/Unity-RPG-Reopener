using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.EventSystems;
using TMPro;

public class InventorySlot : MonoBehaviour, ISelectHandler
{
    [Header("UI Display")]
    [SerializeField] private Image slotImage;
    [SerializeField] private TextMeshProUGUI amountText;
    [SerializeField] private TextMeshProUGUI nameText;
    [SerializeField] private GameObject submenu;
    [Header("Events")]
    [SerializeField] private SignalSender changeSelectedEquipment;
    [SerializeField] private SignalSender equipItem;
    public Item Item { get; private set; }

    public void AssignSlot(Item item, int numHeld)
    {
        Item = item;
        slotImage.sprite = item.ItemIcon;
        nameText.text = item.ItemName;
        amountText.text = numHeld.ToString("n0");
    }

    public void ClearSlot()
    {
        Destroy(this.gameObject); 
    }

    public void OnSelect(BaseEventData eventData)
    {
        changeSelectedEquipment.Raise();
    }

    public void EquipItem()
    {
        equipItem.Raise();
    }

    //display options ex: discard, use, move 
    public void ToggleSubmenu()
    {
        if(submenu.activeInHierarchy)
        {
            submenu.SetActive(false);
        }
        else
        {
            submenu.SetActive(true);
        }
    }
}
