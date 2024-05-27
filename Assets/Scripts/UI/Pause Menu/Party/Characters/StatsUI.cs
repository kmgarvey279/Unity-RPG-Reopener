using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class StatsUI : MonoBehaviour
{
    public Dictionary<IntStatType, int> equipmentComparison = new Dictionary<IntStatType, int>();
    public Dictionary<ElementalProperty, int> resistanceComparison = new Dictionary<ElementalProperty, int>();

    private void Start()
    {
        equipmentComparison.Add(IntStatType.Attack, 0);
        equipmentComparison.Add(IntStatType.Defense, 0);
        equipmentComparison.Add(IntStatType.MAttack, 0);
        equipmentComparison.Add(IntStatType.MDefense, 0);
        equipmentComparison.Add(IntStatType.Agility, 0);
        equipmentComparison.Add(IntStatType.Healing, 0);

        resistanceComparison.Add(ElementalProperty.Fire, 0);
        resistanceComparison.Add(ElementalProperty.Ice, 0);
        resistanceComparison.Add(ElementalProperty.Electric, 0);
        resistanceComparison.Add(ElementalProperty.Dark, 0);
    }
}
