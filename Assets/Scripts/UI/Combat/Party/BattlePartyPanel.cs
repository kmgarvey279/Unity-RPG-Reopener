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


    public void AssignCombatant(AllyCombatant allyCombatant)
    {
        //set basic hp bar
        hpBar.SetInitialValue(allyCombatant.hp.GetValue(), allyCombatant.hp.GetCurrentValue());
        hpNum.text = allyCombatant.hp.GetCurrentValue().ToString();

        //set basic mp bar
        mpBar.SetInitialValue(allyCombatant.mp.GetCurrentValue(), allyCombatant.mp.GetCurrentValue());
        mpNum.text = allyCombatant.mp.GetCurrentValue().ToString();
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
