using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;

public class StatusEffectDisplay : MonoBehaviour
{
    public List<StatusIcon> Icons { get; private set; } = new List<StatusIcon>();
    [SerializeField] private GameObject list;
    [SerializeField] private GameObject iconPrefab;

    public void CreateList(List<StatusEffectInstance> statusEffectInstances)
    {
        ClearIcons();
        if (statusEffectInstances.Count > 0)
        {
            //into multiple lists
            List<StatusEffectInstance> buffList = statusEffectInstances.FindAll(statusInstance => statusInstance.StatusEffect.StatusEffectType == StatusEffectType.Buff);
            foreach (StatusEffectInstance statusEffectInstance in buffList)
            {
                if (statusEffectInstance.StatusEffect.DisplayIcon)
                {
                    AddIcon(statusEffectInstance);
                }
            }

            List<StatusEffectInstance> debuffList = statusEffectInstances.FindAll(statusInstance => statusInstance.StatusEffect.StatusEffectType == StatusEffectType.Debuff);
            foreach (StatusEffectInstance statusEffectInstance in debuffList)
            {
                if (statusEffectInstance.StatusEffect.DisplayIcon)
                {
                    AddIcon(statusEffectInstance);
                }
            }

            List<StatusEffectInstance> otherList = statusEffectInstances.FindAll(statusInstance => statusInstance.StatusEffect.StatusEffectType == StatusEffectType.Other);
            foreach (StatusEffectInstance statusEffectInstance in otherList)
            {
                if (statusEffectInstance.StatusEffect.DisplayIcon)
                {
                    AddIcon(statusEffectInstance);
                }
            }
            list.SetActive(true);
        }
    }

    public void AddIcon(StatusEffectInstance statusEffectInstance)
    {
        GameObject iconObject = Instantiate(iconPrefab, new Vector3(0, 0, 0), Quaternion.identity);
        iconObject.transform.SetParent(list.transform, false);
        
        StatusIcon statusIcon = iconObject.GetComponent<StatusIcon>();
        if (statusIcon != null)
        {
            statusIcon.AssignEffect(statusEffectInstance);
            Icons.Add(statusIcon);
        }
    }

    public void ClearIcons()
    {
        for (int i = Icons.Count - 1; i >= 0; i--)
        {
            StatusIcon thisIcon = Icons[i];
            Icons.Remove(thisIcon);
            Destroy(thisIcon.gameObject);
        }
    }

    public void ToggleInteractivity(bool isInteractive)
    {
        foreach (StatusIcon statusIcon in Icons)
        {
            statusIcon.ToggleButton(isInteractive);
        }
    }

    //public void SelectIcon(StatusIcon icon)
    //{
    //    icon.OnSelect();
    //    infoName.text = icon.StatusEffectInstance.StatusEffect.EffectName;
    //    //infoText.text = icon.StatusEffectInstance.StatusEffect.EffectInfo;
    //}

    //public void DeselectIcon(StatusIcon icon)
    //{
    //    icon.OnDeselect();
    //    infoName.text = "";
    //    infoText.text = "";
    //}
}
