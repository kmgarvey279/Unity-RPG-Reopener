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
    private float healthChange = 0;
    private DamagePopupType currentPopupType;
    public Color damageColor;
    public Color healColor;
    [Header("Clear Popup")]
    public float popupDuration;
    private float popupTimer;

    private void Start()
    {
        popupTimer = popupDuration;
    }

    private void Update()
    {
        //if character has recently taken damage
        if(healthChange > 0)
        {
            //if duration of popup has ended, clear popup/reset timer
            if(popupTimer <= 0)
            {
                ClearPopup();
            }
            //else run timer
            else
            {
                popupTimer -= Time.deltaTime;
            }
        }
    }

    public void UpdatePopup(DamagePopupType popupType, float amount)
    {
        //if new incoming damage/heal isn't 0
        if(amount > 0)
        {
            //if displayed damage/heal isn't 0, add new incoming damage/heal to it and reset timer
            if(healthChange > 0 && popupType == currentPopupType)
            {
                healthChange += amount;
                popupTimer = popupDuration;
            }   
            //else, just display new incoming damage
            else
            {
                healthChange = amount;
                currentPopupType = popupType; 
                SetPopupColor();
            }  
            popupText.text = healthChange.ToString("n0");  
        }
    }

    private void SetPopupColor()
    {
        if(currentPopupType == DamagePopupType.Damage)
        {
            popupText.color = damageColor;     
        }
        else if(currentPopupType == DamagePopupType.Heal)
        {
            popupText.color = healColor;
        }
    }

    private void ClearPopup()
    {
        healthChange = 0;
        popupText.text = "";
        currentPopupType = DamagePopupType.None;
        popupTimer = popupDuration;
    }
}
