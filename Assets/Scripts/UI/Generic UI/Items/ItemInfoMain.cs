using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;
using static UnityEditor.Progress;

public class ItemInfoMain : MonoBehaviour
{
    [SerializeField] private GameObject display;
    [SerializeField] private GameObject header;
    [SerializeField] private TextMeshProUGUI itemNameValue;
    [SerializeField] private TextMeshProUGUI textbox;
    
    public void Display(Item item)
    {
        if (!display.activeInHierarchy)
        {
            display.SetActive(true);
        }

        //name
        if (itemNameValue != null)
        {
            itemNameValue.text = item.ItemName;
        }

        //info
        if (textbox != null)
        {
            textbox.text = item.Description;
        }
    }

    public void Hide()
    {
        if (display.activeInHierarchy)
        {
            display.SetActive(false);
        }

        //name
        if (itemNameValue != null)
        {
            itemNameValue.text = "";
        }

        //info
        if (textbox != null)
        {
            textbox.text = "";
        }
    }
}
