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
    Die,
    Hit
}

public enum BattlefieldSpawnPosition
{
    Target,
    Actor,
    TargetedGridCenter
}

public enum CombatantSpawnPosition
{
    Center,
    Below,
    Above,
    Front,
    Back
}

public enum AnimationTriggerFrequency
{
    Always,
    XHit,
    Even,
    Odd
}


[System.Serializable]
public class ActionAnimationData
{
    [field: Header("Actor Sprite + Animation")]
    [field: SerializeField] public BattleAnimatorTrigger BattleAnimatorTrigger { get; private set; }
    [field: SerializeField] public string CustomAnimatorTrigger { get; private set; } = "";
    [field: SerializeField] public bool ChangeActorOpacity { get; private set; }
    [field: SerializeField] public float NewActorOpacity { get; private set; }
    [field: Header("VFX")]
    [field: SerializeField] public GameObject VFXPrefab { get; private set; }
    [field: SerializeField] public BattlefieldSpawnPosition BattlefieldSpawnPosition { get; private set; }
    [field: SerializeField] public CombatantSpawnPosition CombatantSpawnPosition { get; private set; }
    [field: SerializeField] public bool BindVFXToCombatant { get; private set; } = false;
    [field: SerializeField] public Vector2 SpawnPositionOffset { get; private set; } = new Vector2(0, 0);
    [field: SerializeField] public bool SpawnForEachTarget { get; private set; } = false;
    [field: SerializeField] public bool IsProjectile { get; private set; } = false;
    [field: SerializeField] public bool ReverseProjectile { get; private set; } = false;
    [field: Header("Trigger Frequency")]
    [field: SerializeField] public AnimationTriggerFrequency AnimationTriggerFrequency { get; private set; }
    [field: SerializeField] public int XHit { get; private set; } = 0;
    [field: Header("Timing")]
    [field: SerializeField] public float Delay { get; private set; } = 0;
    [field: SerializeField] public float Duration { get; private set; } = 0.5f;
}
