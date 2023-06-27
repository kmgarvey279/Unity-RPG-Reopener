using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[System.Serializable]
[CreateAssetMenu(fileName = "New Trait", menuName = "Trait")]
public class Trait : ScriptableObject
{
    [field: SerializeField] public string TraitName { get; private set; } = "";
    [field: SerializeField, TextArea(5,10)] public string TraitInfo { get; private set; }
    [field: SerializeField] public Sprite Icon { private set; get; }
    [field: SerializeField] public List<StatModifier> StatModifiers { get; private set; } = new List<StatModifier>();
    [field: SerializeField] public List<ActionModifier> ActionModifiers { get; private set; } = new List<ActionModifier>();
    [field: SerializeField] public List<TriggerableBattleEffect> TriggerableBattleEffects { get; private set; } = new List<TriggerableBattleEffect>();
}
