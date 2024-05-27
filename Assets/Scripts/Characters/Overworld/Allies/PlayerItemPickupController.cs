using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerItemPickupController : MonoBehaviour
{
    [SerializeField] private SignalSender onPickupItem;

    public void GetItem(ItemPickup itemPickup)
    {
        onPickupItem.Raise();
    }
}
