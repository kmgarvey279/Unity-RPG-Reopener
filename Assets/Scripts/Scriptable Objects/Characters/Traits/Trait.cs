using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[System.Serializable]
[CreateAssetMenu(fileName = "New Trait", menuName = "Trait")]
public class Trait : ScriptableObject
{
    public string TraitID { private set; get; } = System.Guid.NewGuid().ToString();

    [field: SerializeField] public string TraitName { get; private set; } = "";
    [field: SerializeField] public string TraitType { get; private set; } = "";
    [field: SerializeField, TextArea(2, 10)] public string Description { get; private set; } = "";
    [field: SerializeField, TextArea(2, 10)] public string SecondaryDescription { get; private set; } = "";
    [field: SerializeField] public Sprite Icon { private set; get; }
    [field: SerializeField] public List<CombatantBool> BoolsToModify { get; private set; } = new List<CombatantBool>();
    [field: SerializeField] public List<IntStatModifier> IntStatModifiers { get; private set; } = new List<IntStatModifier>();
    //[field: SerializeField] public List<FloatStatModifier> FloatStatModifiers { get; private set; } = new List<FloatStatModifier>();
    [field: SerializeField] public List<UniversalModifier> UniversalModifiers { get; private set; } = new List<UniversalModifier>();
    [field: SerializeField] public List<ActionModifier> ActionModifiers { get; private set; } = new List<ActionModifier>();
    [field: SerializeField] public List<StatusEffect> BattleStartStatusEffects { get; private set; } = new List<StatusEffect>();
    [field: SerializeField] public List<PreemptiveBattleEventTrigger> PreemptiveBattleEventTriggers { get; private set; } = new List<PreemptiveBattleEventTrigger>();
    [field: SerializeField] public List<BattleEventTrigger> BattleEventTriggers { get; private set; } = new List<BattleEventTrigger>();
}
