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
        hpBar.SetInitialValue((int)playableCombatant.hp.GetValue(), (int)playableCombatant.hp.GetCurrentValue());
        hpNum.text = playableCombatant.hp.GetCurrentValue().ToString();

        //set basic mp bar
        mpBar.SetInitialValue((int)playableCombatant.mp.GetCurrentValue(), (int)playableCombatant.mp.GetCurrentValue());
        mpNum.text = playableCombatant.mp.GetCurrentValue().ToString();
    }

    public void UpdateHP(float newValue)
    {
        hpBar.DisplayChange(newValue);
    }

    public void ResolveHP()
    {
        hpBar.ResolveChange();
    }

    public void UpdateMP(float newValue)
    {
        mpBar.DisplayChange(newValue);
        mpBar.ResolveChange();
    }
}
