using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Interactable : MonoBehaviour
{
    [HideInInspector] public Leader leader;
    [HideInInspector] public bool leaderInRange = false;
    [HideInInspector] public bool leaderCanInteract = false;
    public bool checkDirection;

    public void Start()
    {
        leader = FindObjectOfType<Leader>();   
    }

    public void Update()
    {
        if(leaderCanInteract)
        {
            if(Input.GetButtonDown("Select"))
            {
                Interact(); 
            }
        }

        if(leaderInRange)
        {
            if(checkDirection)
            {
                Vector3 direction = (transform.position - leader.transform.position).normalized;
                float dot = Vector3.Dot(direction, leader.LookDirection());
                if(dot > 0.5) 
                {
                    leader.DisplayActionPrompt();
                    leaderCanInteract = true;
                }
                else
                {
                    leader.HideActionPrompt();
                    leaderCanInteract = false;
                }
            }
            else 
            {
                leader.DisplayActionPrompt();
                leaderCanInteract = true;
            }
        }    
    }

    public void OnTriggerEnter2D(Collider2D other)
    {
        if(other.gameObject.CompareTag("Playable Character"))
        {
            leaderInRange = true;
        }
    }

    public void OnTriggerExit2D(Collider2D other)
    {
        if(other.gameObject.CompareTag("Playable Character"))
        {
            leaderInRange = false;

            if(leaderCanInteract)
            {
                leader.HideActionPrompt();
                leaderCanInteract = false;
            }
        }
    }

    public virtual void Interact()
    {  
    }
}
