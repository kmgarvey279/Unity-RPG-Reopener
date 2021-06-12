using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class EnemyHealthDisplay : HealthDisplay
{
    public EnemyCombatant combatant;
    public SliderBar sliderBar;

    public override void Start()
    {
        base.Start();
        combatant = GetComponentInParent<EnemyCombatant>();

        sliderBar = GetComponentInChildren<SliderBar>();
        sliderBar.SetSliderValues(combatant.battleStats.health.GetValue(), combatant.battleStats.health.GetCurrentValue());
    }

    public override void HandleHealthChange(DamagePopupType popupType, float amount)
    {
        base.HandleHealthChange(popupType, amount);
        sliderBar.UpdateSlider(combatant.battleStats.health.GetCurrentValue());
    }
}
