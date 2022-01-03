using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class StatusEffectDisplay : MonoBehaviour
{
    public List<StatusIcon> icons = new List<StatusIcon>();
    [SerializeField] private GameObject iconPrefab;

    public void AddStatusIcon(StatusEffectSO statusEffectSO)
    {
        GameObject iconObject = Instantiate(iconPrefab, new Vector3(0, 0, 0), Quaternion.identity);
        iconObject.transform.SetParent(this.transform, false);
        iconObject.GetComponent<StatusIcon>().AssignEffect(statusEffectSO);
        //if buff or if list is empty, insert at start of list
        if(statusEffectSO.isBuff || icons.Count < 1)
        {
            icons.Insert(0, iconObject.GetComponent<StatusIcon>());
            iconObject.transform.SetSiblingIndex(0);
        }
        else
        {
            //search list to find end of buffs/start of debuffs
            for(int i = 0; i < icons.Count; i++)
            {
                if(!icons[i].statusEffectSO.isBuff)
                {
                    icons.Insert(i, iconObject.GetComponent<StatusIcon>());
                    iconObject.transform.SetSiblingIndex(i);
                    break;
                }  
            }
        }
    }

    public void RemoveStatusIcon(StatusEffectSO statusEffectSO)
    {
        foreach(StatusIcon icon in icons)
        {
            if(icon.statusEffectSO == statusEffectSO)
            {
                icons.Remove(icon);
                Destroy(icon.gameObject);
                break; 
            }
        }
    }
}
