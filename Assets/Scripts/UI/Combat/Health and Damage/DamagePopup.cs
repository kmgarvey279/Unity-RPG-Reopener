using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;

public class DamagePopup : MonoBehaviour
{
    [SerializeField] private bool isCrit;
    [Header("Animation")]
    private Animator motionAnimator;
    [Header("UI Elements")]
    [SerializeField] private OutlinedText damageText;
    [Header("Value Displayed")]
    [SerializeField] private Color damageColor;
    [SerializeField] private Color healColor;
    [SerializeField] private Color missColor;
    [SerializeField] private Color buffColor;
    [SerializeField] private Color debuffColor;
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

    public void TriggerHealthPopup(PopupType popupType, string text)
    {
        //set color
        if(!isCrit)
        {
            damageText.SetTextColor(colorDict[popupType]);
        }

        if(popupType == PopupType.Buff || popupType == PopupType.Debuff) 
        {
            text = "[" + text + "]"; 
        }
        damageText.SetText(text);

        motionAnimator.SetTrigger("Activate");

        StartCoroutine(ClearPopupCo());
    }

    private IEnumerator ClearPopupCo()
    {
        yield return new WaitForSeconds(duration);
        Destroy(this.gameObject);
    }
}
