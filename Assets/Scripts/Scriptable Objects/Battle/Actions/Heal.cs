using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu(fileName = "New Action", menuName = "Action/Heal")]
public class Heal : Action
{
    [field: Header("Health Effects")]
    [field: SerializeField] public float Power { get; protected set; } = 1f;
}
