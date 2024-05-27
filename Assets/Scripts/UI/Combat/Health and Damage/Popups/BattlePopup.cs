using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;
using System;
using System.Linq;

public enum PopupType
{
    Heal,
    Damage,
    Miss,
    Mana
}

public class BattlePopup : MonoBehaviour
{
    [Header("Animation")]
    private Animator motionAnimator;
    [Header("UI Elements")]
    [SerializeField] private OutlinedText popupNumber;
    [SerializeField] private OutlinedText popupText;
    [Header("Value Displayed")]
    [SerializeField] private Color damageColor;
    [SerializeField] private Color healColor;
    [SerializeField] private Color missColor;
    [SerializeField] private Color manaColor;
    [SerializeField] private Color critColor;
    private Dictionary<PopupType, Color> colorDict = new Dictionary<PopupType, Color>();
    [Header("Other Text Elements")]
    [SerializeField] private GameObject weakText;
    [SerializeField] private GameObject critText;

    private void OnEnable()
    {
        motionAnimator = GetComponent<Animator>();
        colorDict.Add(PopupType.Damage, damageColor);
        colorDict.Add(PopupType.Heal, healColor);
        colorDict.Add(PopupType.Miss, missColor);
        colorDict.Add(PopupType.Mana, manaColor);

        popupNumber.SetText("");
        popupText.SetText("");
    }

    public void TriggerNumber(PopupType popupType, CombatantType combatantType, int amount, bool isCrit, bool isVulnerable, float duration)
    {
        //set colors
        if (isVulnerable)
        {
            weakText.SetActive(true);
        }

        if (isCrit)
        {
            popupNumber.SetTextColor(critColor);
            critText.SetActive(true);
        }
        else
        {
            popupNumber.SetTextColor(colorDict[popupType]);
        }

        popupNumber.SetText(amount.ToString());

        if (popupType == PopupType.Damage)
        {
            if (combatantType == CombatantType.Player)
            {
                motionAnimator.SetTrigger("PlayerDamage");
            }
            else if (combatantType == CombatantType.Enemy) 
            {
                motionAnimator.SetTrigger("EnemyDamage");
            }
        }
        else
        {
            motionAnimator.SetTrigger("Heal");
        }

        StartCoroutine(ClearPopupCo(duration));
    }

    public void TriggerText(PopupType popupType, string text, float duration)
    {
        popupText.SetTextColor(colorDict[popupType]);
        popupText.SetText(text);
        motionAnimator.SetTrigger("Text");

        StartCoroutine(ClearPopupCo(duration));
    }

    private IEnumerator ClearPopupCo(float duration)
    {
        yield return new WaitForSeconds(duration);
        Destroy(this.gameObject);
    }
}
