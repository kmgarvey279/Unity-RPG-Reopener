using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using TMPro;

public class DialogueBox : MonoBehaviour
{
    [SerializeField] private GameObject panel;
    [SerializeField] private TextMeshProUGUI textField;
    [SerializeField] private Image portrait;

    private List<string> testText = new List<string>{"Part 1...", "part 2...", "part 3."};
    private int currentLine = 0;

    [SerializeField] SignalSenderInt onChangeOverworldState;

    public void DisplayTextBox(string text)
    {
        panel.SetActive(true);
        textField.text = testText[0];
        onChangeOverworldState.Raise((int)OverworldStateType.Dialogue);
    }

    public void AdvanceText()
    {
        if(currentLine >= testText.Count - 1)
        {
            HideTextBox();
        }
        else
        {
            currentLine += 1;
            textField.text = testText[currentLine];
        }
    }

    private void HideTextBox()
    {
        panel.SetActive(false);
        textField.text = "";
        currentLine = 0;
        onChangeOverworldState.Raise((int)OverworldStateType.FreeMove);
    }
}
