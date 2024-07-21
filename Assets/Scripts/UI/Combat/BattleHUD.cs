using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class BattleHUD : MonoBehaviour
{
    [SerializeField] private Mask hudMask;

    [SerializeField] private DialogueManager dialogueManager;

    [SerializeField] private VictoryDisplay victoryDisplay;

    public void DisplayDialogue(DialogueData dialogueData)
    {
        hudMask.enabled = true;
        dialogueManager.SetDialogueData(dialogueData);
    }

    public void OnEndDialogue()
    {
        hudMask.enabled = false;
    }

    public void DisplayVictoryScreen(List<ExpData> activeCharacters, List<ExpData> reserveCharacters, int expGain)
    {
        hudMask.enabled = true;
        victoryDisplay.Display(activeCharacters, reserveCharacters, expGain);
    }
}
