using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[System.Serializable]
public class EnemyAI
{
    //actions
    [field: SerializeField] public List<WeightedAction> WeightedActions { get; private set; }
    [field: SerializeField] public List<Action> LastActions { get; private set; }

    public EnemyAI(EnemyInfo enemyInfo)
    {
        WeightedActions = enemyInfo.WeightedActions;
        LastActions = new List<Action>();
    }
}
