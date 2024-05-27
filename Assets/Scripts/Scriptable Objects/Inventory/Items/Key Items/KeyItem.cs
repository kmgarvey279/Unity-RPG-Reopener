using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu(fileName = "New Key Item", menuName = "Inventory/Items/Key Item")]
public class KeyItem : Item
{
    //
    public void OnEnable()
    {
        ItemType = ItemType.Key;
    }
}
