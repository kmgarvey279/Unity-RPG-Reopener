using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using TMPro;

public class UnitHealthDisplay : MonoBehaviour
{
    public CharacterInfo characterInfo;
    public DamagePopup damagePopup;

    public virtual void Start()
    {
        characterInfo = GetComponentInParent<Character>().characterInfo;
        damagePopup = GetComponentInChildren<DamagePopup>();
    }

    public virtual void HandleHealthChange(DamagePopupType popupType, float amount)
    {
        damagePopup.UpdatePopup(popupType, amount);
    }
}
