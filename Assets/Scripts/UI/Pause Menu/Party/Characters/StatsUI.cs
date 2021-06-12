using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class StatsUI : MonoBehaviour
{
    public Dictionary<StatType, float> equipmentComparison = new Dictionary<StatType, float>();

    private void Start()
    {
        equipmentComparison.Add(StatType.Health, 0);
        equipmentComparison.Add(StatType.Mana, 0);
        equipmentComparison.Add(StatType.Attack, 0);
        equipmentComparison.Add(StatType.Defense, 0);
        equipmentComparison.Add(StatType.Special, 0);
    }
}
