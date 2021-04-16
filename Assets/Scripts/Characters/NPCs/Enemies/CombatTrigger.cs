using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CombatTrigger : MonoBehaviour
{
    [SerializeField] private SignalSenderGO targetAdd;
    [SerializeField] private SignalSenderGO targetRemove;
    [SerializeField] private string otherTag;

    private void OnTriggerEnter2D(Collider2D other)
    {
        if(other.gameObject.CompareTag(otherTag))
        {
            targetAdd.Raise(this.gameObject);
        }
    }

    private void OnTriggerExit2D(Collider2D other)
    {
        if(other.gameObject.CompareTag(otherTag))
        {
            targetRemove.Raise(this.gameObject);
        }
    }
}
