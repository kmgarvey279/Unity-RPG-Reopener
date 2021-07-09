using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Room : MonoBehaviour
{
    [SerializeField] private GameObject virtualCamera;
    [SerializeField] private GameObject[] objects;

    public void OnTriggerEnter2D(Collider2D other)
    {
        if(other.CompareTag("Playable Character"))
        {
            virtualCamera.SetActive(true);
            Debug.Log("Enter");
            for (int i = 0; i < objects.Length; i++)
            {
                objects[i].SetActive(true);
            }
        }
    }

    public void OnTriggerExit2D(Collider2D other)
    {
        if(other.CompareTag("Playable Character"))
        {
            virtualCamera.SetActive(false);
            Debug.Log("Exit");
            for (int i = 0; i < objects.Length; i++)
            {
                objects[i].SetActive(false);
            }
        }
    }
}
