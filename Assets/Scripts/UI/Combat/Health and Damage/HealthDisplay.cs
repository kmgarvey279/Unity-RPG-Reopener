using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using TMPro;

public class HealthDisplay : MonoBehaviour
{
    public DamagePopup damagePopup;

    public virtual void Start()
    {
        damagePopup = GetComponentInChildren<DamagePopup>();
    }

    public virtual void HandleHealthChange(DamagePopupType popupType, float amount)
    {
        damagePopup.TriggerPopup(popupType, amount);
    }
}
