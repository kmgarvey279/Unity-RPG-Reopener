using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Events;

public class ItemPickup : Interactable
{
    [Header("Item")]
    [SerializeField] private ItemObject item;

    private void OnEnable()
    {
        effectedTags = new string[]{"Player"};
    }

    public override void TriggerInteraction()
    {
        if(item != null)
        {
            
            Destroy(this.gameObject);
        }
    }
}