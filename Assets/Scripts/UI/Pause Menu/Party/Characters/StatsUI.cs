using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class StatsUI : MonoBehaviour
{
    public Dictionary<StatType, int> equipmentComparison = new Dictionary<StatType, int>();
    public Dictionary<ElementalProperty, int> resistanceComparison = new Dictionary<ElementalProperty, int>();

    private void Start()
    {
        equipmentComparison.Add(StatType.Attack, 0);
        equipmentComparison.Add(StatType.Defense, 0);
        equipmentComparison.Add(StatType.MAttack, 0);
        equipmentComparison.Add(StatType.MDefense, 0);
        equipmentComparison.Add(StatType.Agility, 0);
        equipmentComparison.Add(StatType.HP, 0);
        equipmentComparison.Add(StatType.MP, 0);

        resistanceComparison.Add(ElementalProperty.Fire, 0);
        resistanceComparison.Add(ElementalProperty.Ice, 0);
        resistanceComparison.Add(ElementalProperty.Electric, 0);
        resistanceComparison.Add(ElementalProperty.Dark, 0);
    }
}
