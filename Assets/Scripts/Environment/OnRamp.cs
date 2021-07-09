using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class OnRamp : MonoBehaviour
{
    public void OnTriggerEnter2D(Collider2D other)
    {
        if(other.gameObject.CompareTag("Playable Character"))
        {
            Leader player = other.GetComponent<Leader>();
            player.sprite.sortingOrder = player.sprite.sortingOrder + 1;
            player.gameObject.layer = player.gameObject.layer + 1;
        }
    }
}
