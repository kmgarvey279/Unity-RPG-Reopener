using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu(fileName = "New Usable Item Object", menuName = "Inventory/Items/Usable Item")]
public class UsableItemObject : ItemObject
{
    [SerializeField]  private int healthRecovery;
    
    public virtual void OnEnable()
    {
        itemType = ItemType.Usable;
    }


    public virtual void Use(){}
}
