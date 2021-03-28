using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using TMPro;

public class InventorySlot : MonoBehaviour
{

    [Header("Item In Slot")]
    public ItemObject item;
    [Header("UI Display")]
    [SerializeField] private Image slotImage;
    [SerializeField] private TextMeshProUGUI amountText;
    [SerializeField] private TextMeshProUGUI nameText;
    [SerializeField] private GameObject submenu;

    public void AssignSlot(ItemObject itemObject, int numHeld)
    {
        item = itemObject;
        slotImage.sprite = item.icon;
        nameText.text = item.name;
        amountText.text = numHeld.ToString("n0");
    }

    public void ClearSlot()
    {
        Destroy(this.gameObject); 
    }

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
