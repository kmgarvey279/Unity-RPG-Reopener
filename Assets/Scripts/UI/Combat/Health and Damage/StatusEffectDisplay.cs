using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;

public class StatusEffectDisplay : MonoBehaviour
{
    public List<StatusIcon> icons = new List<StatusIcon>();
    [SerializeField] private GameObject list;
    [SerializeField] private GameObject iconPrefab;
    [SerializeField] private TextMeshProUGUI infoName;
    [SerializeField] private TextMeshProUGUI infoText;

    public void CreateList(Combatant combatant)
    {
        ClearIcons();
        if(combatant.StatusEffectInstances.Count > 0)
        {
            list.SetActive(true);
            foreach (StatusEffectInstance statusEffectInstance in combatant.StatusEffectInstances)
            {
                AddIcon(statusEffectInstance);
            }
        }
    }

    public void AddIcon(StatusEffectInstance statusEffectInstance)
    {
        GameObject iconObject = Instantiate(iconPrefab, new Vector3(0, 0, 0), Quaternion.identity);
        iconObject.transform.SetParent(list.transform, false);
        iconObject.GetComponent<StatusIcon>().AssignEffect(statusEffectInstance);
        //if buff or if list is empty, insert at start of list
        if(statusEffectInstance.StatusEffect.StatusEffectType != StatusEffectType.Debuff || icons.Count < 1)
        {
            icons.Insert(0, iconObject.GetComponent<StatusIcon>());
            iconObject.transform.SetSiblingIndex(0);
        }
        else
        {
            //search list to find end of buffs/start of debuffs
            for(int i = 0; i < icons.Count; i++)
            {
                if(icons[i].StatusEffectInstance.StatusEffect.StatusEffectType == StatusEffectType.Debuff)
                {
                    icons.Insert(i, iconObject.GetComponent<StatusIcon>());
                    iconObject.transform.SetSiblingIndex(i);
                    break;
                }  
            }
        }
    }

    public void ClearIcons()
    {
        for(int i = icons.Count - 1; i >= 0; i--)
        {
            StatusIcon thisIcon = icons[i];
            icons.Remove(thisIcon);
            Destroy(thisIcon.gameObject);
        }
    }

    public void SelectIcon(StatusIcon icon)
    {
        icon.OnSelect();
        infoName.text = icon.StatusEffectInstance.StatusEffect.EffectName;
        infoText.text = icon.StatusEffectInstance.StatusEffect.EffectInfo;
    }

    public void DeselectIcon(StatusIcon icon)
    {
        icon.OnDeselect();
        infoName.text = "";
        infoText.text = "";
    }
}
