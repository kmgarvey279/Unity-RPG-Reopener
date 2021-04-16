using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class GetObjectsInRange : MonoBehaviour
{
    [SerializeField] private string otherTag;
    private List<GameObject> objects = new List<GameObject>();

    private void OnTriggerEnter2D(Collider2D other)
    {
        if(other.gameObject.CompareTag(otherTag) && other.isTrigger)
        {
            if(!objects.Contains(other.gameObject))
            {
                objects.Add(other.gameObject);
            }
        }
    }

    private void OnTriggerExit2D(Collider2D other)
    {
        if(other.gameObject.CompareTag(otherTag) && other.isTrigger)
        {
            RemoveObject(other.gameObject);
        }
    }

    public void RemoveObject(GameObject gameObject)
    {
        if(objects.Contains(gameObject))
        {
            objects.Remove(gameObject);
        }
    }

    public List<GameObject> GetObjectList()
    {
        return objects;
    }

}
