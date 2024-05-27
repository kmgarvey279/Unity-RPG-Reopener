using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class DontDestroyContainer : MonoBehaviour
{
    public static DontDestroyContainer instance;

    private void Awake()
    {
        if (instance == null)
        {
            instance = new DontDestroyContainer();
            DontDestroyOnLoad(gameObject);
        }
        else
        {
            Destroy(gameObject);
        }
    }
}
