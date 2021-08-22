using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using TMPro;

public class HealthDisplay : MonoBehaviour
{
    private DamagePopup damagePopup;
    private Combatant combatant;
    private float barTickSpeed = 0.05f;
    [SerializeField] private GameObject healthBarContainer;
    [SerializeField] private SliderBar healthBar;
    [SerializeField] private SliderBar damageBar;
    [SerializeField] private SliderBar healBar;
    [SerializeField] private SliderBar miasmaBar;

    private void Start()
    {
        combatant = GetComponentInParent<Combatant>();
        damagePopup = GetComponentInChildren<DamagePopup>();

        //set normal health bar
        healthBar.SetMaxValue(combatant.battleStats.health.GetValue());
        healthBar.SetCurrentValue(combatant.battleStats.health.GetCurrentValue());
        //set damage bar
        damageBar.SetMaxValue(combatant.battleStats.health.GetValue());
        damageBar.SetCurrentValue(combatant.battleStats.health.GetCurrentValue());
        //set heal bar
        healBar.SetMaxValue(combatant.battleStats.health.GetValue());
        //set miasma bar
        miasmaBar.SetMaxValue(combatant.battleStats.health.GetValue());
    }

    public void ToggleBarVisibility(bool isActive)
    {
        healthBarContainer.SetActive(isActive);
    }

    public void HandleHealthChange(DamagePopupType popupType, int amount)
    {
        damagePopup.TriggerPopup(popupType, amount);
        if(popupType == DamagePopupType.Damage)
        {
            StartCoroutine(DisplayDamage(amount));
        }
        else if(popupType == DamagePopupType.Heal)
        {
            StartCoroutine(DisplayHeal(amount));
        }
    }

    private IEnumerator DisplayDamage(int amount)
    {
        int healthBarValue = combatant.battleStats.health.GetCurrentValue();
        healthBar.SetCurrentValue(healthBarValue);
        int damageBarValue = combatant.battleStats.health.GetCurrentValue() + amount;

        while(damageBarValue > healthBarValue) 
        {
            yield return new WaitForSeconds(barTickSpeed);
            damageBarValue = damageBarValue - 1;
            damageBar.SetCurrentValue(damageBarValue);
        }
    }

    private IEnumerator DisplayHeal(int amount)
    {
        int healthBarValue = combatant.battleStats.health.GetCurrentValue() - amount;
        int healBarValue = combatant.battleStats.health.GetCurrentValue();
        healBar.SetCurrentValue(healBarValue);

        while(healthBarValue < healBarValue) 
        {
            yield return new WaitForSeconds(barTickSpeed);
            healthBarValue = healthBarValue++;
            healthBar.SetCurrentValue(healthBarValue);
        }
    }

    public void Clear()
    {
        damagePopup.ClearPopup();
        ToggleBarVisibility(false);
    }
}
