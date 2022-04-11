using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using TMPro;

public class HealthDisplay : MonoBehaviour
{
    private Combatant combatant;

    [SerializeField] private AnimatedBar healthBar;
    [SerializeField] private DamagePopup damagePopup;

    private void Start()
    {
        combatant = GetComponentInParent<Combatant>();
        damagePopup = GetComponentInChildren<DamagePopup>();

        healthBar.SetInitialValue(combatant.hp.GetValue(), combatant.hp.GetCurrentValue());
    }

    public void DisplayHPBar()
    {
        healthBar.gameObject.SetActive(true);
    }

    public void HandleHealthChange(DamagePopupType popupType, int amount)
    {
        DisplayHPBar();
        damagePopup.TriggerPopup(popupType, amount);
        healthBar.UpdateBar(combatant.hp.GetCurrentValue());
        StartCoroutine(HideHPBar());
    }

    private IEnumerator HideHPBar()
    {
        yield return new WaitForSeconds(1f);
        healthBar.gameObject.SetActive(false);
    }


    // private IEnumerator DisplayDamage(int amount)
    // {
    //     // healthBar.IncreaseBar(amount);
    //     // int healthBarValue = combatant.hp.GetCurrentValue();
    //     // int damageBarValue = combatant.hp.GetCurrentValue() + amount;
    //     // int damageBarValue = healBarValue;
    //     // healthBar.SetDamage(damageBarValue);
    //     // healthBar.SetHealth(healthBarValue);
    //     // if(combatant is PlayableCombatant)
    //     //     battlePartyPanel.UpdateStatusBar(StatusBarType.HP, recoveryBarValue);

    //     // while(damageBarValue > healthBarValue) 
    //     // {
    //     //     yield return new WaitForSeconds(barTickSpeed);
    //     //     damageBarValue = damageBarValue - 1;
    //     //     // damageBar.SetCurrentValue(damageBarValue);
    //     //     healthBar.SetDamage(damageBarValue);
    //     //     if(combatant is PlayableCombatant)
    //     //         battlePartyPanel.UpdateStatusBar(StatusBarType.HPDamage, recoveryBarValue);
    //     // }
    //     // healBar.SetDamage(0);
    // }

    // private IEnumerator DisplayHeal(int amount)
    // {
    //     // int healthBarValue = combatant.hp.GetCurrentValue() - amount;
    //     // int recoveryBarValue = combatant.hp.GetCurrentValue();
    //     // // healBar.SetCurrentValue(healBarValue);
    //     // healthBar.SetRecovery(recoveryBarValue);
    //     // if(combatant is PlayableCombatant)
    //     //     battlePartyPanel.UpdateStatusBar(StatusBarType.HPRecovery, recoveryBarValue);
    //     //     // onChangeRecovery.Raise(combatant.gameObject, recoveryBarValue);

    //     // while(healthBarValue < recoveryBarValue) 
    //     // {
    //     //     yield return new WaitForSeconds(barTickSpeed);
    //     //     healthBarValue = healthBarValue++;
    //     //     // healthBar.SetCurrentValue(healthBarValue);
    //     //     healthBar.SetHealth(healthBarValue);
    //     //     if(combatant is PlayableCombatant)
    //     //         battlePartyPanel.UpdateStatusBar(StatusBarType.HP, recoveryBarValue);
    //     // }
    //     // healBar.SetRecovery(0);
    // }

    public void Clear()
    {
        damagePopup.ClearPopup();
        // ToggleBarVisibility(false);
    }
}
