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
        sliderBar.SetSliderValues(character.characterInfo.health.GetValue(), character.characterInfo.health.GetCurrentValue());
        sliderBar.gameObject.SetActive(false);
    }

    public override void HandleHealthChange(DamagePopupType popupType, float amount)
    {
        base.HandleHealthChange(popupType, amount);
        sliderBar.UpdateSlider(character.characterInfo.health.GetCurrentValue());
    }

    public void ToggleHealthBar(bool isActive)
    {
        sliderBar.gameObject.SetActive(isActive);
    }
}
