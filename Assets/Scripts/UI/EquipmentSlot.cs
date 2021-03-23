using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using TMPro;

public class EquipmentSlot : MonoBehaviour
{
    [Header("Equipment Type")]
    public EquipmentType equipmentType;
    [SerializeField] private Sprite emptySprite;
    [Header("UI Display")]
    [SerializeField] private Image slotImage;
    [SerializeField] private TextMeshProUGUI nameText;
    [Header("Equipment In Slot")]
    [SerializeField] private EquipmentObject equipment;

    private void OnEnable()
    {
        AssignSlot();
    }

    public void AssignSlot()
    {
        if(equipment != null)
        {
            slotImage.sprite = equipment.icon;
            nameText.text = equipment.name;
        }
        else
        {
            slotImage.sprite = emptySprite;  
            nameText.text = string.Empty;  
        }
    }

    public void ClearSlot()
    {
        equipment = null;
        slotImage.sprite = emptySprite;
        nameText.text = string.Empty; 
    }
}
