using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Room : MonoBehaviour
{
    [SerializeField] private GameObject virtualCamera;
    [SerializeField] private GameObject[] objects;

    public void ActivateRoom()
    {
        virtualCamera.SetActive(true);
        for (int i = 0; i < objects.Length; i++)
        {
            objects[i].SetActive(true);
        }
    }

    public void DeactivateRoom()
    {
        virtualCamera.SetActive(false);
        for (int i = 0; i < objects.Length; i++)
        {
            objects[i].SetActive(false);
        }
    }
}
