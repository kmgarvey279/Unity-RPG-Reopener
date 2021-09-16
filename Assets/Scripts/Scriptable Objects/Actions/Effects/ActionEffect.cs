using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[System.Serializable]
public class ActionEffect : ScriptableObject
{
    public virtual void ApplyEffect(Action action, Combatant attacker, Combatant target)
    {
    }
}
