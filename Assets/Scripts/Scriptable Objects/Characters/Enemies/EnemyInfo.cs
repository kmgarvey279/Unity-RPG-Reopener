using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[System.Serializable]
public class WeightedAction
{
    public Action action;
    [SerializeField] private WeightType weightType;
    [System.Serializable]
    private enum WeightType
    {
        VeryLow,
        Low,
        Mid,
        High,
        VeryHigh
    }
    private Dictionary<WeightType, int> weightDict = new Dictionary<WeightType, int>()
    {
        {WeightType.VeryLow, 10},
        {WeightType.Low, 20},
        {WeightType.Mid, 30},
        {WeightType.High, 40},
        {WeightType.VeryHigh, 50}
    };
    public int BaseWeight()
    {
        return weightDict[weightType];
    }
}

[CreateAssetMenu(fileName = "New Enemy Info", menuName = "Character Info/Enemy")]
public class EnemyInfo : CharacterInfo
{
    [Header("Skills")]
    public List<WeightedAction> weightedActions = new List<WeightedAction>();

    protected override void OnEnable()
    {
        base.OnEnable();
    }
}
