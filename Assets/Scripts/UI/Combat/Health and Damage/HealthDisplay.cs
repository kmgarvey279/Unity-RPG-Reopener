using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using TMPro;

public enum PopupType
{
    Heal, 
    Damage,
    Buff,
    Debuff,
    Miss
}

public class HealthDisplay : MonoBehaviour
{
    private Combatant combatant;

    [SerializeField] private GameObject display;
    [SerializeField] private AnimatedBar healthBar;
    [SerializeField] private StatusEffectDisplay statusEffectDisplay;
    [SerializeField] private GameObject damagePopupPrefab;
    [SerializeField] private GameObject popupSpawnPoint;

    private void Start()
    {
        combatant = GetComponentInParent<Combatant>();
        healthBar.SetInitialValue(combatant.hp.GetValue(), combatant.hp.GetCurrentValue());
    }

    public void Display(bool show)
    {
        display.gameObject.SetActive(show);
    }

    public void DisplayMessage(PopupType popupType, string message)
    {
        GameObject damagePopupObject = Instantiate(damagePopupPrefab, popupSpawnPoint.transform.position, Quaternion.identity);
        damagePopupObject.transform.SetParent(popupSpawnPoint.transform);
        damagePopupObject.GetComponent<DamagePopup>().TriggerMessagePopup(popupType, message);
    }

    public void DisplayHealthChange(PopupType popupType, float amount, bool isCrit)
    {
        Debug.Log("displaying popup for " + amount);
        // if(!display.gameObject.activeInHierarchy)
        // {
        //     Display(true);
        // }

        GameObject damagePopupObject = Instantiate(damagePopupPrefab, popupSpawnPoint.transform.position, Quaternion.identity);
        damagePopupObject.transform.SetParent(popupSpawnPoint.transform);
        damagePopupObject.GetComponent<DamagePopup>().TriggerHealthPopup(popupType, Mathf.Abs(amount), isCrit);

        healthBar.DisplayChange(combatant.hp.GetCurrentValue());
    }

    public void ResolveHealthChange()
    {
        healthBar.ResolveChange();
    }

    public void AddStatusIcon(StatusEffectSO statusEffectSO)
    {
        statusEffectDisplay.AddStatusIcon(statusEffectSO);
    }

    public void RemoveStatusIcon(StatusEffectSO statusEffectSO)
    {
        statusEffectDisplay.RemoveStatusIcon(statusEffectSO);
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
}
