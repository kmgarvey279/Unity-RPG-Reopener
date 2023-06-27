using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class NPC : Interactable
{
    [Header("GameObject Components")]
    public Animator animator;
    [Header("Dialogue")]
    private string dialogue;
    [Header("Event")]
    [SerializeField] private SignalSenderString onTriggerDialogue;

    public override void Interact()
    {  
        onTriggerDialogue.Raise("Test...");
    }
}
