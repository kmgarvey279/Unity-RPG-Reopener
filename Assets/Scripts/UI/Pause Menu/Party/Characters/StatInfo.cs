using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;

public class StatInfo : MonoBehaviour
{
    [SerializeField] private StatType statType;
    private CharacterInfoUI characterInfoUI;
    private int stat;
    [SerializeField] private TextMeshProUGUI valueText;
    [SerializeField] private TextMeshProUGUI modifiedValueText;
    public Color statUpColor;
    public Color statDownColor;


    private void Start()
    {
        characterInfoUI = GetComponentInParent<CharacterInfoUI>();
        stat = characterInfoUI.playableCharacterInfo.Stats[statType].CurrentValue;
        valueText.text = stat.ToString("n0");
    }

    private void DisplayModifiedValue(float change)
    {
        float modifiedValue = stat + change;
        if (modifiedValue > stat)
        {
            modifiedValueText.color = statUpColor;
        }
        else if (modifiedValue < stat)
        {
            modifiedValueText.color = statDownColor;
        }
    }
}
