using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;

public class VictoryDisplay : MonoBehaviour
{
    [SerializeField] private GameObject display;
    [SerializeField] private TextMeshProUGUI expValue;
    [SerializeField] private List<ExpPanel> activePanels = new List<ExpPanel>();
    [SerializeField] private List<ExpPanel> reservePanels = new List<ExpPanel>();

    public void Display(List<ExpData> activeCharacters, List<ExpData> reserveCharacters, int expGain)
    {
        display.SetActive(true);
        expValue.text = expGain.ToString();

        for (int i = 0; i < activePanels.Count; i++)
        {
            if (activeCharacters.Count > i)
            {
                activePanels[i].SetValues(activeCharacters[i].Icon, activeCharacters[i].LevelStart, activeCharacters[i].LevelEnd, activeCharacters[i].CurrentEXP, activeCharacters[i].NextLevelRequirement);
            }
            else
            {
                activePanels[i].gameObject.SetActive(false);
            }
        }

        for (int i = 0; i < reservePanels.Count; i++)
        {
            if (reserveCharacters.Count > i)
            {
                reservePanels[i].SetValues(activeCharacters[i].Icon, activeCharacters[i].LevelStart, activeCharacters[i].LevelEnd, activeCharacters[i].CurrentEXP, activeCharacters[i].NextLevelRequirement);
            }
            else
            {
                reservePanels[i].gameObject.SetActive(false);
            }
        }
    }
}
