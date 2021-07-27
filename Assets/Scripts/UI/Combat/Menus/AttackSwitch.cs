using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public enum AttackType 
{
    Melee,
    Gun
}

public class AttackSwitch : MonoBehaviour
{
    [SerializeField] private Image meleeIcon;
    private bool canUseMelee; 
    [SerializeField] private Image gunIcon;
    private bool canUseGun;
    [SerializeField] private Color selectedColor;
    [SerializeField] private Color deselectedColor;

    public void ChangeSelectedIcon(AttackType attackType)
    {
        if(attackType == AttackType.Melee)
        {
            meleeIcon.color = selectedColor;
            gunIcon.color = deselectedColor;
        } 
        else
        {
            gunIcon.color = selectedColor;
            meleeIcon.color = deselectedColor;      
        }
    }
}
