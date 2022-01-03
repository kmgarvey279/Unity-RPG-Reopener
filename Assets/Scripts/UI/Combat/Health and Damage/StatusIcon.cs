using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class StatusIcon : MonoBehaviour
{
    public StatusEffectSO statusEffectSO;
    [SerializeField] private Image iconFrame;
    [SerializeField] private Image iconImage;
    [SerializeField] private Color buffColor;
    [SerializeField] private Color debuffColor;
    
    public void AssignEffect(StatusEffectSO statusEffectSO)
    {
        this.statusEffectSO = statusEffectSO;
        iconImage = statusEffectSO.icon;
        if(statusEffectSO.isBuff)
        {
            iconFrame.color = buffColor;
        }
        else
        {
            iconFrame.color = debuffColor;
        }
    }
}
