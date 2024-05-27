using System.Collections;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;

public class DontDestroyObject : MonoBehaviour
{
    [HideInInspector]
    public string objectID;

    private void Awake()
    {
        objectID = name + transform.position.ToString();
    }

    private void Start()
    {
        List<DontDestroyObject> dontDestroylist = FindObjectsOfType<DontDestroyObject>().ToList();
        for (int i = 0; i < dontDestroylist.Count; i++)
        {
            if (dontDestroylist[i] != this && dontDestroylist[i].objectID == objectID)
            {
                Destroy(gameObject);
            }
        }
        DontDestroyOnLoad(gameObject);
    }
}
