using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;

public class DamagePopup : MonoBehaviour
{
    private Animator animator;
    [Header("UI Elements")]
    [SerializeField] private TextMeshProUGUI popupText;
    [Header("Value Displayed")]
    [SerializeField] private Color damageColor;
    [SerializeField] private Color healColor;
    // [SerializeField] private Color buffColor;
    // [SerializeField] private Color debuffColor;
    [SerializeField] private Color missColor;
    private Dictionary<PopupType, Color> colorDict = new Dictionary<PopupType, Color>();
    [Header("Duration")]
    [SerializeField] private float duration;

    private void OnEnable()
    {
        animator = GetComponent<Animator>();
        colorDict.Add(PopupType.Damage, damageColor);
        colorDict.Add(PopupType.Heal, healColor);
        // colorDict.Add(PopupType.Buff, buffColor);
        // colorDict.Add(PopupType.Debuff, debuffColor);
        colorDict.Add(PopupType.Miss, missColor);
    }

    public void TriggerMessagePopup(PopupType popupType, string message)
    {
        popupText.color = colorDict[popupType]; 
        popupText.text = message;
        animator.SetTrigger("Activate");
        StartCoroutine(ClearPopupCo());
    }

    public void TriggerHealthPopup(PopupType popupType, float amount, bool isCrit)
    {
        Debug.Log("popup has spawned");
        popupText.color = colorDict[popupType]; 
        if(isCrit)
        {
            popupText.fontSize = popupText.fontSize + 0.15f;
        }
        popupText.text = amount.ToString();
        animator.SetTrigger("Activate");
        StartCoroutine(ClearPopupCo());
    }

    private IEnumerator ClearPopupCo()
    {
        yield return new WaitForSeconds(duration);
        Destroy(this.gameObject);
    }
}
