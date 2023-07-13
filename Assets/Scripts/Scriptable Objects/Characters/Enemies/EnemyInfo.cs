using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[System.Serializable]
public class WeightedAction
{
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

    [SerializeField] private WeightType weightType;
    [field: SerializeField] public Action Action { private set; get; }
    
    public int BaseWeight()
    {
        return weightDict[weightType];
    }

    public void SetAction(Action action)
    {
        Action = action;
    }
}

[CreateAssetMenu(fileName = "New Enemy Info", menuName = "Character Info/Enemy")]
public class EnemyInfo : CharacterInfo
{
    [Header("Skills")]
    [field: SerializeField] public List<WeightedAction> WeightedActions = new List<WeightedAction>();

    private float baseBlockPower = 0.2f;

    protected override void OnEnable()
    {
        base.OnEnable();

        SecondaryStats[SecondaryStatType.BlockPower].UpdateBaseValue(baseBlockPower);
    }
}
