using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;

public class BattlePopup : MonoBehaviour
{
    [Header("Animation")]
    private Animator motionAnimator;
    [Header("UI Elements")]
    [SerializeField] private OutlinedText popupText;
    [Header("Value Displayed")]
    [SerializeField] private Color damageColor;
    [SerializeField] private Color healColor;
    [SerializeField] private Color missColor;
    [SerializeField] private Color buffColor;
    [SerializeField] private Color debuffColor;
    [SerializeField] private Color critColor;
    private Dictionary<PopupType, Color> colorDict = new Dictionary<PopupType, Color>();
    [Header("Duration")]
    [SerializeField] private float duration;

    private void OnEnable()
    {
        motionAnimator = GetComponent<Animator>();
        colorDict.Add(PopupType.Damage, damageColor);
        colorDict.Add(PopupType.Heal, healColor);
        colorDict.Add(PopupType.Miss, missColor);
        colorDict.Add(PopupType.Buff, buffColor);
        colorDict.Add(PopupType.Debuff, debuffColor);
    }

    public void Trigger(PopupType popupType, CombatantType combatantType, string text, bool isCrit)
    {
        //set color
        if (isCrit)
        {
            popupText.SetTextColor(critColor);
        }
        else
        {
            popupText.SetTextColor(colorDict[popupType]);
        }

        if(popupType == PopupType.Buff || popupType == PopupType.Debuff) 
        {
            text = "[" + text + "]"; 
        }
        popupText.SetText(text);

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
            motionAnimator.SetTrigger("NonDamage");
        }

        StartCoroutine(ClearPopupCo());
    }

    private IEnumerator ClearPopupCo()
    {
        yield return new WaitForSeconds(duration);
        Destroy(this.gameObject);
    }
}
