using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;
using UnityEngine.UI;

public class ExpPanel : MonoBehaviour
{
    [SerializeField] private Image icon;
    [SerializeField] private TextMeshProUGUI levelStartValue;
    [SerializeField] private GameObject levelEndContainer;
    [SerializeField] private TextMeshProUGUI levelEndValue;
    [SerializeField] private AnimatedBar expBar;


    public void SetValues(Sprite sprite, int levelStart, int levelEnd, int currentEXP, int neededEXP)
    {
        icon.sprite = sprite;
        levelStartValue.text = levelStart.ToString();
        levelEndValue.text = levelEnd.ToString();
        if (levelStart != levelEnd)
        {
            levelEndContainer.SetActive(true);
            levelEndValue.text = levelEnd.ToString();
        }
        else
        {
            levelEndContainer.SetActive(false);
        }

        expBar.SetInitialValue(neededEXP, currentEXP);
    }
    //public void SetValues(PlayableCharacterID playableCharacterID, int expGain)
    //{
    //    PlayableCombatantRuntimeData charData = SaveManager.Instance.LoadedData.GetPlayableCombatantRuntimeData(playableCharacterID);
    //    int levelStart = charData.Level;

    //    charData.GainEXP(expGain);
    //    int levelEnd = charData.Level;

    //    int currentEXP = charData.CurrentEXP;
    //    int neededEXP = charData.exp

    //    icon.sprite = sprite;
    //    currentLevelValue.text = currentLevel.ToString();
    //    if (nextLevel != currentLevel)
    //    {
    //        nextLevelContainer.SetActive(true);
    //        nextLevelValue.text = nextLevel.ToString();
    //    }
    //    else
    //    {
    //        nextLevelContainer.SetActive(false);
    //    }

    //    expBar.SetInitialValue();
    //}
}
