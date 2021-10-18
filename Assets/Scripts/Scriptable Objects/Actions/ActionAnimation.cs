using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[System.Serializable]
public enum GraphicType
{
    //spawn at user position
    Cast,
    //spawn at user position and move to target position
    Projectile,
    //spawn at target position
    Effect,
    //do nothing
    None
}

[System.Serializable]
[CreateAssetMenu(fileName = "New Action Animation", menuName = "Action/Action Animation")]
public class ActionAnimation : ScriptableObject
{
    [Header("User Animation")]
    public string animatorTrigger;
    [Header("Effect Graphics")]
    public GraphicType graphicType;
    public float graphicSpawnDelay;
    [Header("Duration")]
    public float duration;
    [Header("Movement")]
    public bool moveUser;
    public bool moveTarget;
    public float moveSpeed;
}
