using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;
using UnityEngine.EventSystems;

public class DialogueManager : MonoBehaviour
{
    [SerializeField] private GameObject display;

    [SerializeField] private TextMeshProUGUI dialogueTMP;
    private DialogueData dialogueData;
    private int currentSpeakerIndex = 0;
    private int currentLineIndex = 0;

    [SerializeField] private GameObject speakerBox;
    [SerializeField] private OutlinedText speakerName;

    private void OnEnable()
    {
        InputManager.Instance.OnPressAdvance.AddListener(AdvanceDialogue);
    }

    private void OnDisable()
    {
        InputManager.Instance.OnPressAdvance.RemoveListener(AdvanceDialogue);
    }

    private void Awake()
    {
        if (display.activeInHierarchy)
        {
            display.SetActive(false);
        }
    }

    //private void Update()
    //{
    //    //if (InputManager.InputLocked)
    //    //{
    //    //    return;
    //    //}

    //    //if (InputManager.ConfirmMenuInput)
    //    //{
    //    //    //Debug.Log("line index: " + currentLineIndex);
    //    //    //Debug.Log("speaker index: " + currentSpeakerIndex);
    //    //    AdvanceDialogue();
    //    //}

    //}

    public void SetDialogueData(DialogueData _dialogueData)
    {
        Time.timeScale = 0;
        InputManager.Instance.ChangeActionMap(ActionMapType.Dialogue);

        display.SetActive(true);

        dialogueData = _dialogueData;
        currentSpeakerIndex = 0;
        currentLineIndex = 0;

        DisplayDialogue();
    }

    private void DisplayDialogue()
    {
        if (dialogueData == null)
        {
            EndDialogue();
            return;
        }

        SpeakerData speakerData = dialogueData.Speakers[currentSpeakerIndex];

        if (speakerData.SpeakerName != "")
        {
            speakerBox.SetActive(true);
            speakerName.SetText(speakerData.SpeakerName);
        }
        else
        {
            speakerBox.SetActive(false);
            speakerName.SetText("");
        }

        dialogueTMP.SetText(speakerData.Lines[currentLineIndex]);
    }

    private void AdvanceDialogue(bool isPressed)
    {        
        if (currentLineIndex < dialogueData.Speakers[currentSpeakerIndex].Lines.Count - 1)
        {
            currentLineIndex++;
            DisplayDialogue();
        }
        else if (currentSpeakerIndex < dialogueData.Speakers.Count - 1)
        {
            currentSpeakerIndex++;
            currentLineIndex = 0;
            DisplayDialogue();
        }
        else
        {
            EndDialogue();
        }
    }

    private void EndDialogue()
    {
        dialogueData = null;
        display.SetActive(false);

        Time.timeScale = 1;
        InputManager.Instance.ChangeActionMap(ActionMapType.Overworld);
    }
}
