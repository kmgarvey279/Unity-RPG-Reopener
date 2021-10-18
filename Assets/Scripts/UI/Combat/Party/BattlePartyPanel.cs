using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using TMPro;

public class BattlePartyPanel : MonoBehaviour
{
    [Header("Character portrait")]
    [SerializeField] private Image portrait;
    [Header("HP Bar")]
    [SerializeField] private TextMeshProUGUI hpNum;
    [SerializeField] private AnimatedBar hpBar;
    [Header("MP Bar")]
    [SerializeField] private TextMeshProUGUI mpNum;
    [SerializeField] private AnimatedBar mpBar;


    public void AssignCombatant(PlayableCombatant playableCombatant)
    {
        //set basic hp bar
        hpBar.SetInitialValue(playableCombatant.hp.GetValue(), playableCombatant.hp.GetCurrentValue());
        hpNum.text = playableCombatant.hp.GetCurrentValue().ToString();

        //set basic mp bar
        mpBar.SetInitialValue(playableCombatant.mp.GetCurrentValue(), playableCombatant.mp.GetCurrentValue());
        mpNum.text = playableCombatant.mp.GetCurrentValue().ToString();
    }

    public void UpdateHP(int newValue)
    {
        hpBar.UpdateBar(newValue);
    }

    public void UpdateMP(int newValue)
    {
        mpBar.UpdateBar(newValue);
    }
}
