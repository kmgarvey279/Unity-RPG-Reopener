using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class NPC : Interactable
{
    [Header("GameObject Components")]
    public Rigidbody2D rigidbody;
    public Animator animator;
    [Header("Dialogue")]
    private string dialogue;
    [Header("Event")]
    [SerializeField] private SignalSenderString triggerDialogue;

    public override void Interact()
    {  
        triggerDialogue.Raise("Test...");
    }
}
