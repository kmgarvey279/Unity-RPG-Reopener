using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Targeter : MonoBehaviour
{
    public GameObject currentTarget;
    public List<GameObject> targets = new List<GameObject>();
    public string targetTag;

    // Start is called before the first frame update
    void Start()
    {
        targets.AddRange(GameObject.FindGameObjectsWithTag(targetTag));
        currentTarget = targets[0];   
    }

    public void ChangeCurrentTarget(GameObject newTarget)
    {
        currentTarget = newTarget;
    }
}
