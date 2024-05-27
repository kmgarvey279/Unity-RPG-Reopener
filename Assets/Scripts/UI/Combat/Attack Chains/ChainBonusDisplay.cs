using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ChainBonusDisplay : MonoBehaviour
{
    //private CombatantType combatantType = CombatantType.None;
    [SerializeField] private OutlinedText chainText;
    [SerializeField] private OutlinedText chainAmount;
    [SerializeField] private Color playerFill;
    [SerializeField] private Color playerNumberFill;
    [SerializeField] private Color playerOutline;
    [SerializeField] private Color defaultFill;
    [SerializeField] private Color defaultOutline;
    [SerializeField] private GameObject glowEffect;

    public void UpdateDisplay(float newValue)
    {
        if (newValue <= 0)
        {
            chainText.SetTextColor(defaultFill);
            chainText.SetSecondaryTextColor(defaultOutline);
            chainAmount.SetTextColor(defaultFill);
            chainAmount.SetSecondaryTextColor(defaultOutline);
            if (glowEffect.activeInHierarchy)
            {
                glowEffect.SetActive(false);
            }
        }
        else 
        {
            chainText.SetTextColor(playerFill);
            chainText.SetSecondaryTextColor(playerOutline);
            chainAmount.SetTextColor(playerNumberFill);
            chainAmount.SetSecondaryTextColor(playerOutline);
            if(!glowEffect.activeInHierarchy)
            {
                glowEffect.SetActive(true);
            }
        }
        chainAmount.SetText("x" + newValue);
    }
}
