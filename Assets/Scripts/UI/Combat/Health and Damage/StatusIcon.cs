using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class StatusIcon : MonoBehaviour
{
    public StatusEffect statusEffect;
    [SerializeField] private Image iconFrame;
    [SerializeField] private Image iconImage;
    [SerializeField] private Color buffColor;
    [SerializeField] private Color debuffColor;
    
    public void AssignEffect(StatusEffect statusEffect)
    {
        this.statusEffect = statusEffect;
        iconImage = statusEffect.icon;
        if(statusEffect.isBuff)
        {
            iconFrame.color = buffColor;
        }
        else
        {
            iconFrame.color = debuffColor;
        }
    }
}
