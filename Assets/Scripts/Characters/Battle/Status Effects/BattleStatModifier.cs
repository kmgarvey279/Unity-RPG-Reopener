using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[System.Serializable]
public class BattleStatModifier
{
    public BattleStatType statToModify;
    public int additive = 0;
    public float multiplier = 1;
}
