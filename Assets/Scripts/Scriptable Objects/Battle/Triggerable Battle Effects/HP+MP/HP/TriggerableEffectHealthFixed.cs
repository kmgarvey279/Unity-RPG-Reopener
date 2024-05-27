using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu(fileName = "NewTriggerableEffect", menuName = "Triggerable Effects/Health Effect/Fixed")]

public class TriggerableEffectHealthFixed : TriggerableEffect
{
    [Header("Battle Effect Health Fixed")]
    [SerializeField] private bool isHeal;
    public override void ApplyEffect(Combatant actor, Combatant target, float value)
    {
        //clamp value to int
        int intValue = Mathf.Clamp(Mathf.FloorToInt(value), 1, 9999);
        
        if (isHeal)
        {
            target.OnHealed(intValue, false);
        }
        else
        {
            target.OnAttacked(intValue, false, false);
        }
    }
}