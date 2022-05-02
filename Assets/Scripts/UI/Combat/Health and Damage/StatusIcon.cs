using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class StatusIcon : MonoBehaviour
{
    public StatusEffectSO statusEffectSO;
    [SerializeField] private Sprite iconFrame;
    [SerializeField] private Sprite iconImage;
    
    public void AssignEffect(StatusEffectSO statusEffectSO)
    {
        this.statusEffectSO = statusEffectSO;
        iconImage = statusEffectSO.icon;
    }
}
