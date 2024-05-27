using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;

public class EquipmentStatDisplay : MonoBehaviour
{
    [SerializeField] private GameObject statPanelPrefab;
    [SerializeField] private GameObject statPanelParent;
    private List<StatPanel> statPanels = new List<StatPanel>();
    private Dictionary<IntStatType, string> statText = new Dictionary<IntStatType, string>()
    {
        { IntStatType.Attack, "Attack" },
        { IntStatType.Defense, "Defense" },
        { IntStatType.MAttack, "M. Attack" },
        { IntStatType.MDefense, "M. Defense" },
        { IntStatType.Healing, "Healing" },
        { IntStatType.Agility, "Agility" },
        { IntStatType.CritRate, "Crit. Rate" },
        { IntStatType.EvadeRate, "Evade Rate" }
    };

    public void DisplayEquipmentStats(EquipmentItem equipmentItem)
    {
        Clear();
        foreach (IntStatModifier intStatModifier in equipmentItem.IntStatModifiers)
        {
            GameObject statPanelObject = Instantiate(statPanelPrefab, Vector3.zero, Quaternion.identity);
            statPanelObject.transform.SetParent(statPanelParent.transform, false);
            StatPanel statPanel = statPanelObject.GetComponent<StatPanel>();
            if (statPanel != null)
            {
                bool isPercentage = false;
                if (intStatModifier.IntStatType == IntStatType.CritRate || intStatModifier.IntStatType == IntStatType.CritRate)
                {
                    isPercentage = true;
                }
                statPanel.SetLabel(statText[intStatModifier.IntStatType]);
                statPanel.SetValue(Mathf.FloorToInt(intStatModifier.Modifier), isPercentage);
            }
            statPanels.Add(statPanel);
        }
    }

    public void Clear()
    {
        for (int i = statPanels.Count - 1; i >= 0; i--)
        {
            Destroy(statPanels[i].gameObject);
        }
        statPanels.Clear();
    }
}
