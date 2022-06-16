using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public enum ActionEffectType
{
    Damage, 
    Heal,
    ApplyStatusEffect,
    RemoveStatusEffects,
    Move,
    UseItem,
    Defend,
    Knockback,
    Other
}

[System.Serializable]
public class ActionEffect : ScriptableObject
{
    public ActionEffectType actionEffectType;

    public virtual void ApplyEffect(ActionEvent actionEvent)
    {
    }
}
