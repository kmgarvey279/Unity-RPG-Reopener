using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Events;

public class ItemPickup : Interactable
{
    private ItemContainer itemContainer;
    [field: Header("Item")]
    [field: SerializeField] public Item Item { get; private set; }

    private void Start()
    {
        itemContainer = GetComponentInParent<ItemContainer>();
    }

    public override void Interact()
    {
        itemContainer.OnGetItem();
    }
}