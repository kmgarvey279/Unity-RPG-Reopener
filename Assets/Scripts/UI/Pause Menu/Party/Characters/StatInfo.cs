using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;

public class StatInfo : MonoBehaviour
{
    [SerializeField] private StatType statType;
    private CharacterInfoUI characterInfoUI;
    private Stat stat;
    [SerializeField] private TextMeshProUGUI valueText;
    [SerializeField] private TextMeshProUGUI modifiedValueText;
    public Color statUpColor;
    public Color statDownColor;


    private void Start()
    {
        characterInfoUI = GetComponentInParent<CharacterInfoUI>();
        stat = characterInfoUI.playableCharacterInfo.statDict[statType];
        valueText.text = stat.GetValue().ToString("n0");
    }

    private void DisplayModifiedValue(float change)
    {
        float modifiedValue = stat.GetValue() + change;
        if(modifiedValue > stat.GetValue())
        {
            modifiedValueText.color = statUpColor;  
        }
        else if(modifiedValue < stat.GetValue())
        {

        }
    }
}
