using System;
using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;

public class ItemInfo : MonoBehaviour
{
    [Header("Text")]
    [SerializeField] private TextMeshProUGUI itemNameValue;
    [SerializeField] private TextMeshProUGUI itemTypeValue;
    [SerializeField] private TextBox textBox;
    [SerializeField] private bool displaySecondaryText;
    [Header("Stats")]
    [SerializeField] private bool displayStats;
    [SerializeField] private EquipmentStatDisplay equipmentStatDisplay;
    [Header("Character Compatability")]
    [SerializeField] private bool displayCompatablity;
    [SerializeField] private CharacterEquipCompatabilityPanel compatabilityPanel;


    public void DisplayItem(Item item)
    {
        Clear();

        if (item == null)
        {
            return;
        }

        //text
        itemNameValue.text = item.ItemName;
        if (displaySecondaryText)
        {
            textBox.SetText(item.Description, item.SecondaryDescription);
        }
        else
        {
            textBox.SetText(item.Description);
        }

        //show stats + character compatability if it is equipable
        if (item is EquipmentItem)
        {
            EquipmentItem equipmentItem = (EquipmentItem)item;
            if (displayStats)
            {
                equipmentStatDisplay.DisplayEquipmentStats(equipmentItem);
            }
            if (displayCompatablity)
            {
                compatabilityPanel.DisplayItemCompatability(equipmentItem);
            }
        }
        else
        {
            equipmentStatDisplay.Clear();
            compatabilityPanel.Hide();
        }
    }

    public void Clear()
    {
        itemNameValue.text = "------";
        itemTypeValue.text = "";
        textBox.Clear();
    }
}
