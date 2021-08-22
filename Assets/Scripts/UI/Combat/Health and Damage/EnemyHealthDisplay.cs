using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class EnemyHealthDisplay : HealthDisplay
{
    // private Combatant combatant;
    // [SerializeField] private GameObject healthBarContainer;
    // [SerializeField] private SliderBar healthBar;
    // [SerializeField] private SliderBar damageBar;
    // [SerializeField] private SliderBar miasmaBar;

    // public override void Start()
    // {
    //     base.Start();
    //     combatant = GetComponentInParent<Combatant>();

    //     healthBar.SetMaxValue(combatant.battleStats.health.GetValue());
    //     healthBar.SetCurrentValue(combatant.battleStats.health.GetCurrentValue());
    // }

    // public void ToggleBarVisibility(bool isActive)
    // {
    //     healthBarContainer.SetActive(isActive);
    // }

    // public override void HandleHealthChange(DamagePopupType popupType, float amount)
    // {
    //     base.HandleHealthChange(popupType, amount);
    //     healthBar.SetCurrentValue(combatant.battleStats.health.GetCurrentValue());
    // }
}
