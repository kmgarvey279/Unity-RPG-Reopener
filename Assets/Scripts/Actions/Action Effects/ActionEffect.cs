using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public enum ActionEffectType
{
    Damage, 
    Heal,
    ApplyStatusEffect,
    RemoveStatusEffects,
    Knockback,
    Other
}

[System.Serializable]
public class ActionEffect
{
    [Range(1,100)] public float successRate = 0; 
    public bool applyToActor = false;

    public void TriggerEffect(ActionEvent actionEvent)
    {
        if(Roll(successRate))
        {
            ApplyEffect(actionEvent);
        }
    }

    public virtual void ApplyEffect(ActionEvent actionEvent)
    {
    }

    public bool Roll(float chance)
    {
        float roll = Random.Range(1, 100);
        if(roll <= chance)
        {
            return true;
        }
        else
        {
            return false;
        }
    }
}
