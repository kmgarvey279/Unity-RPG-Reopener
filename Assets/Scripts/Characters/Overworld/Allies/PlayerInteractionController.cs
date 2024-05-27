using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerInteractionController : MonoBehaviour
{
    [SerializeField] private GameObject actionPrompt;
    public Interactable Interactable { get; private set; }
   

    public void OnTriggerEnter2D(Collider2D other)
    {
        if (other.gameObject.CompareTag("Interactable"))
        {
            Interactable = other.gameObject.GetComponent<Interactable>();
            DisplayActionPrompt();
        }
    }

    public void OnTriggerExit2D(Collider2D other)
    {
        if (other.gameObject.CompareTag("Interactable"))
        {
            if (Interactable)
            {
                Interactable = null;
            }
            HideActionPrompt();
        }
    }

    public void DisplayActionPrompt()
    {
        if (!actionPrompt.activeInHierarchy)
            actionPrompt.SetActive(true);
    }

    public void HideActionPrompt()
    {
        if (actionPrompt.activeInHierarchy)
            actionPrompt.SetActive(false);
    }
}
