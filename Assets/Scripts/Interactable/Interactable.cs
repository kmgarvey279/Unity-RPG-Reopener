using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Interactable : MonoBehaviour
{
    public string[] effectedTags;

    private void OnTriggerEnter2D(Collider2D other)
    {
        for (int i = 0; i < effectedTags.Length; i++)
        {
            if(other.gameObject.CompareTag(effectedTags[i]))
            {
                TriggerInteraction();
                return;
            }   
        }
    }

    // Update is called once per frame
    public virtual void TriggerInteraction()
    {  
    }
}
