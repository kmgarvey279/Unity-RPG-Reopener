using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public enum Conditional
{
    If,
    And, 
    Or
}

[System.Serializable]
public class BattleConditionContainer
{
    [field: SerializeField] public Conditional Conditional { get; protected set; }
    [field: SerializeField] public bool IsNot { get; protected set; }
    [field: SerializeField] public BattleCondition BattleCondition { get; protected set; }
}
