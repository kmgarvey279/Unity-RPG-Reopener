using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[System.Serializable]
[CreateAssetMenu(fileName = "NewTriggerableEffect", menuName = "Triggerable Effects/No Effect")]
public class TriggerableEffect : ScriptableObject
{
    public virtual void ApplyEffect(Combatant actor, Combatant target, float value)
    {
    }
}
