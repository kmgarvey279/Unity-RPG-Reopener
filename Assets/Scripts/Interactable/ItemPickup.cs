using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Events;

public class ItemPickup : Interactable
{
    [Header("Item")]
    [SerializeField] private Inventory inventory;
    [SerializeField] private Item item;
    [SerializeField] private SignalSenderString itemGet;

    public override void Interact()
    {
        //inventory.AddItem(item);
        //itemGet.Raise(item.itemId);   
        //Destroy(this.gameObject);
    }
}