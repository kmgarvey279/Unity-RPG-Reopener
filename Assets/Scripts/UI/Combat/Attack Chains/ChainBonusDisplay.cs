using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ChainBonusDisplay : MonoBehaviour
{
    private CombatantType combatantType = CombatantType.None;
    [SerializeField] private OutlinedText chainText;
    [SerializeField] private OutlinedText chainAmount;
    [SerializeField] private Color playerFill;
    [SerializeField] private Color playerNumberFill;
    [SerializeField] private Color playerOutline;
    [SerializeField] private Color enemyFill;
    [SerializeField] private Color enemyNumberFill;
    [SerializeField] private Color enemyOutline;
    [SerializeField] private Color defaultFill;
    [SerializeField] private Color defaultOutline;
    [SerializeField] private GameObject glowEffect;

    public void UpdateDisplay(float newValue, CombatantType newCombatantType)
    {
        if (combatantType != newCombatantType)
        {
            combatantType = newCombatantType;

            if (combatantType == CombatantType.Player)
            {
                chainText.SetTextColor(playerFill);
                chainText.SetSecondaryTextColor(playerOutline);
                chainAmount.SetTextColor(playerNumberFill);
                chainAmount.SetSecondaryTextColor(playerOutline);
            }
            else if (combatantType == CombatantType.Enemy)
            {
                chainText.SetTextColor(enemyFill);
                chainText.SetSecondaryTextColor(enemyOutline);
                chainAmount.SetTextColor(enemyNumberFill);
                chainAmount.SetSecondaryTextColor(enemyOutline);
            }
            else
            {
                chainText.SetTextColor(defaultFill);
                chainText.SetSecondaryTextColor(defaultOutline);
                chainAmount.SetTextColor(defaultFill);
                chainAmount.SetSecondaryTextColor(defaultOutline);
            }
        }
        if (newValue <= 1f && glowEffect.activeInHierarchy)
        {
            glowEffect.SetActive(false);
        }
        else if (newValue > 1f && !glowEffect.activeInHierarchy)
        {
            glowEffect.SetActive(true);
        }
        chainAmount.SetText(newValue.ToString("F2"));
    }
}
