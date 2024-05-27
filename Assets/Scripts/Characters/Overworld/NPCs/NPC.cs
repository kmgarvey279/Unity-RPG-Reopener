using Pathfinding;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class NPC : Interactable
{
    private NPCMovement npcMovement;
    [Header("Dialogue")]
    [SerializeField] private DialogueData dialogueData;
    //[Header("Event")]
    //[SerializeField] private SignalSenderString onTriggerDialogue;

    public void Awake()
    {
        npcMovement = GetComponent<NPCMovement>();
    }

    public override void Interact()
    {
        Player player = FindObjectOfType<Player>();
        if (player)
        {
            Vector2 playerDirection = (player.transform.position - transform.position).normalized;
            npcMovement.SetDirection(playerDirection);
        }

        if (dialogueData != null && dialogueData.Speakers.Count > 0)
        {
            DialogueManager.Instance.SetDialogueData(dialogueData);
        }
    }
}
