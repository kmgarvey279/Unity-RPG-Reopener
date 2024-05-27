using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;
using UnityEngine.UI;

public class StatusInfo : MonoBehaviour
{
    [SerializeField] private TextMeshProUGUI header;
    [SerializeField] private TextMeshProUGUI content;

    //private void OnEnable()
    //{
    //    statusColors = new Dictionary<StatusEffectType, Color>();
    //    statusColors.Add(StatusEffectType.Buff, buffTextColor);
    //    statusColors.Add(StatusEffectType.Debuff, debuffTextColor);
    //    statusColors.Add(StatusEffectType.Other, otherTextColor);
    //}

    public void DisplayStatusEffects(StatusEffect statusEffect)
    {
        header.SetText(statusEffect.EffectName);
        content.SetText(statusEffect.Description);
        ////clear previous text
        //Clear();
        //foreach (StatusEffect statusEffect in statusEffects)
        //{
        //    //create header panel
        //    GameObject header = Instantiate(textboxHeaderPrefab, Vector3.zero, Quaternion.identity);
        //    header.transform.SetParent(textboxParent.transform, false);
        //    textboxes.Add(header);
        //    TextMeshProUGUI nameTMP = header.GetComponentInChildren<TextMeshProUGUI>();
        //    if (nameTMP != null)
        //    {
        //        nameTMP.text = "<color=\"white\">" + statusEffect.EffectName + "</color>";
        //    }

        //    //create empty panel
        //    GameObject textbox = Instantiate(textboxPrefab, Vector3.zero, Quaternion.identity);
        //    textbox.transform.SetParent(textboxParent.transform, false);
        //    textboxes.Add(textbox);

        //    //populate panel with info
        //    foreach (string effectDescription in statusEffect.EffectDescriptions)
        //    {
        //        GameObject infoText = Instantiate(textPrefab, Vector3.zero, Quaternion.identity);
        //        infoText.transform.SetParent(textbox.transform, false);
        //        TextMeshProUGUI infoTMP = infoText.GetComponent<TextMeshProUGUI>();
        //        if (infoTMP != null)
        //        {
        //            infoTMP.text = effectDescription;
        //        }
        //    }

        //    //create break
        //    GameObject textboxBreak = Instantiate(textboxBreakPrefab, Vector3.zero, Quaternion.identity);
        //    textboxBreak.transform.SetParent(textboxParent.transform, false);
        //    textboxes.Add(textboxBreak);
        //}
    }

    public void Clear()
    {
        //clear previous text
        //for (int i = textboxes.Count - 1; i >= 0; i--)
        //{
        //    Destroy(textboxes[i]);
        //}
    }
}
