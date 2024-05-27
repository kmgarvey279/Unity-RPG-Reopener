using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class RoomConnector : MonoBehaviour
{
    [SerializeField] private Vector2 direction;

    public void OnTriggerEnter2D(Collider2D other)
    {
        if (other.CompareTag("PlayerChangeRoom"))
        {
            Player player = other.GetComponentInParent<Player>();
            if (player != null)
                StartCoroutine(player.ChangeRoomCo(direction));
        }
    }
}
