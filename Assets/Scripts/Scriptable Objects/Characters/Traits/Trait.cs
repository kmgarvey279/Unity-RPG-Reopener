using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[System.Serializable]
[CreateAssetMenu(fileName = "New Trait", menuName = "Trait")]
public class Trait : ScriptableObject
{
    public string traitName;
    [TextArea(5,10)]
    public string effectInfo;
    public List<TriggerableSubEffect> triggerableSubEffects;

    public TraitInstance CreateInstance(Combatant combatant, Trait trait)
    {
        return new TraitInstance(combatant, trait);
    }
}
