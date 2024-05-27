using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ItemContainer : MonoBehaviour
{
    [field: SerializeField] public GuidWrapper ItemInstanceID { get; private set; }
    [field: SerializeField] public ItemPickup ItemPickup { get; private set; }

    //public ItemContainer()
    //{
    //    ItemInstanceID = new GuidWrapper();
    //}

    //private void OnEnable()
    //{
    //    Debug.Log("Item ID: " + ItemInstanceID.Guid.ToString());
    //}

    public void OnGetItem()
    {
        ItemInteractionManager.GetItem(this);
    }

    public void DeactivateItem()
    {
        ItemPickup.gameObject.SetActive(false);
    }
}
