using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class EnemyHealthDisplay : UnitHealthDisplay
{
    public SliderBar sliderBar;

    public override void Start()
    {
        base.Start();
        sliderBar = GetComponentInChildren<SliderBar>();
        sliderBar.SetSliderValues(characterInfo.maxHealth.GetValue(), characterInfo.currentHealth);
    }

    public override void HandleHealthChange(DamagePopupType popupType, float amount)
    {
        base.HandleHealthChange(popupType, amount);
        sliderBar.UpdateSlider(characterInfo.currentHealth);
    }
}
