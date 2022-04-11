using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Entrance : MonoBehaviour
{
    private Connector connector;

    private void Start()
    {
        connector = GetComponentInParent<Connector>();
    } 

    public void OnTriggerEnter2D(Collider2D other)
    {
        if(other.CompareTag("Player"))
        {
            connector.PlayerEnter(other.gameObject);
        }
    }
}
