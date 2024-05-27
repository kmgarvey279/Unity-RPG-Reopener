using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class RoomBoundary : MonoBehaviour
{
    [SerializeField] private Room room;

    private void OnTriggerEnter2D(Collider2D other)
    {
        if(other.CompareTag("PlayerChangeRoom"))
        {
            room.ActivateRoom(other.gameObject);
        }
    }

    private void OnTriggerExit2D(Collider2D other)
    {
        if(other.CompareTag("PlayerChangeRoom"))
        {
            room.DeactivateRoom();
        }
    }
}
