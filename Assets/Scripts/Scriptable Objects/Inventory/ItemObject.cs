using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu(fileName = "New Item Object", menuName = "Inventory/Items/Generic Item")]
public class ItemObject: ScriptableObject
{
    public string itemName = "New Item";
    public Sprite icon = null;
    public int numHeld = 0;

    public virtual void Use(){}
}
