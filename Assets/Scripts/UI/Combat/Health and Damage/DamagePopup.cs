using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;

public enum DamagePopupType
{
    Damage,
    Heal,
    Miss
}

public class DamagePopup : MonoBehaviour
{
    private Animator animator;
    [Header("UI Elements")]
    [SerializeField] private TextMeshProUGUI popupText;
    [Header("Value Displayed")]
    public Color damageColor;
    public Color healColor;
    public Color missColor;

    private void Start()
    {
        animator = GetComponent<Animator>();
    }

    public void TriggerPopup(DamagePopupType popupType, float amount)
    {
        switch((int)popupType)
        {
        case 0:
            popupText.color = damageColor; 
            popupText.text = amount.ToString("n0");
            break;
        case 1:
            popupText.color = healColor; 
            popupText.text = amount.ToString("n0");
            break;
        case 2:
            popupText.color = missColor; 
            popupText.text = "MISS";
            break;
        default:
            break;
        }
        animator.SetTrigger("Activate");
    }

    public void ClearPopup()
    {
        popupText.text = "";
    }
}
