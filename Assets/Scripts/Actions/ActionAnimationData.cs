using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public enum BattleAnimatorTrigger
{
    None,
    Custom,
    Melee,
    Ranged,
    Magic,
    Cast,
    Stun,
    Defend,
    Move,
    Die
}


[System.Serializable]
public class ActionAnimationData
{
    [field: Header("Actor Animation")]
    [field: SerializeField] public BattleAnimatorTrigger BattleAnimatorTrigger { get; private set; }
    [field: SerializeField] public string CustomAnimatorTrigger { get; private set; } = "";
    [field: Header("VFX")]
    [field: SerializeField] public GameObject VFXPrefab { get; private set; }
    [field: SerializeField] public ActionVFXPosition VFXSpawnPosition { get; private set; }
    [field: SerializeField] public CombatantBindPosition CombatantBindPosition { get; private set; }
    [field: SerializeField] public bool BindVFXToSpawnPoint { get; private set; } = false;
    [field: SerializeField] public bool SpawnForEachTarget { get; private set; } = false;
    [field: SerializeField] public bool IsProjectile { get; private set; } = false;
    [field: Header("Trigger Frequency")]
    [field: SerializeField] public bool OnlyPlayOnXHit { get; private set; } = false;
    [field: SerializeField] public int XHit { get; private set; } = 0;
    [field: Header("Timing")]
    [field: SerializeField] public float Delay { get; private set; } = 0;
    [field: SerializeField] public float Duration { get; private set; } = 0.5f;
}
