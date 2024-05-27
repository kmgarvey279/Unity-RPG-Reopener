using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Events;

[System.Serializable]
public class ColliderEvent : UnityEvent<Collider2D> { }

public class ColliderSignalSender : MonoBehaviour
{
    [SerializeField] private List<string> tagsToCheckFor = new List<string>();
    public ColliderEvent onTriggerEnter;
    public ColliderEvent onTriggerStay;
    public ColliderEvent onTriggerExit;

    private void OnTriggerEnter2D(Collider2D other)
    {
        if (gameObject.activeInHierarchy && tagsToCheckFor.Contains(other.gameObject.tag))
        {
            Debug.Log("Matching Object Entered Collider");
            onTriggerEnter.Invoke(other);
        }
    }

    private void OnTriggerExit2D(Collider2D other)
    {
        if (gameObject.activeInHierarchy && tagsToCheckFor.Contains(other.gameObject.tag))
        {
            Debug.Log("Matching Object Exited Collider");
            onTriggerExit.Invoke(other);
        }
    }

    private void OnTriggerStay2D(Collider2D other)
    {
        if (gameObject.activeInHierarchy && tagsToCheckFor.Contains(other.gameObject.tag))
        {
            Debug.Log("Matching Object Entered Collider");
            onTriggerStay.Invoke(other);
        }
    }
}
