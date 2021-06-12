using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;

public enum DamagePopupType
{
    None,
    Damage,
    Heal
}

public class DamagePopup : MonoBehaviour
{
    [Header("UI Elements")]
    [SerializeField] private TextMeshProUGUI popupText;
    [Header("Value Displayed")]
    public Color damageColor;
    public Color healColor;
    private float popupDuration = 1f;

    public void TriggerPopup(DamagePopupType popupType, float amount)
    {
        SetPopupColor(popupType);
        popupText.text = amount.ToString("n0");
        StartCoroutine(ClearPopupCo());
    }

    public IEnumerator ClearPopupCo()
    {
        yield return new WaitForSeconds(popupDuration);
        popupText.enabled = false;
        popupText.text = "";
    }

    private void SetPopupColor(DamagePopupType popupType)
    {
        if(popupType == DamagePopupType.Damage)
        {
            popupText.color = damageColor;     
        }
        else if(popupType == DamagePopupType.Heal)
        {
            popupText.color = healColor;
        }
    }
}
