using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using TMPro;

public class UnitHealthDisplay : MonoBehaviour
{
    public Character character;
    public DamagePopup damagePopup;

    public virtual void Start()
    {
        character = GetComponentInParent<Character>();
        damagePopup = GetComponentInChildren<DamagePopup>();
    }

    public virtual void HandleHealthChange(DamagePopupType popupType, float amount)
    {
        damagePopup.UpdatePopup(popupType, amount);
    }
}
