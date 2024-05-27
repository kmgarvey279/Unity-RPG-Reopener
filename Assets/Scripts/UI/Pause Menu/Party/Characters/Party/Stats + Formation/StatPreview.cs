using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;

public class StatPreview : MonoBehaviour
{
    [SerializeField] private GameObject content;
    [Header("HP/MP")]
    [SerializeField] private StatPanelPreview hpPanel;
    [SerializeField] private StatPanelPreview mpPanel;
    [Header("Stats")]
    [SerializeField] private StatPanelPreview atkPanel;
    [SerializeField] private StatPanelPreview defPanel;
    [SerializeField] private StatPanelPreview mAtkPanel;
    [SerializeField] private StatPanelPreview mDefPanel;
    [SerializeField] private StatPanelPreview healPanel;
    [SerializeField] private StatPanelPreview aglPanel;
    [SerializeField] private StatPanelPreview critPanel;
    [SerializeField] private StatPanelPreview evadePanel;
    private Dictionary<IntStatType, StatPanelPreview> panelDict = new Dictionary<IntStatType, StatPanelPreview>();
    //[Header("Elemental Resistances")]
    //[SerializeField] private StatPanel fireResistValue;
    //[SerializeField] private StatPanel iceResistValue;
    //[SerializeField] private StatPanel electricResistValue;
    //[SerializeField] private StatPanel darkResistValue;

    public void Awake()
    {
        panelDict.Add(IntStatType.MaxHP, hpPanel);
        panelDict.Add(IntStatType.MaxMP, mpPanel);
        panelDict.Add(IntStatType.Attack, atkPanel);
        panelDict.Add(IntStatType.MAttack, mAtkPanel);
        panelDict.Add(IntStatType.Defense, defPanel);
        panelDict.Add(IntStatType.MDefense, mDefPanel);
        panelDict.Add(IntStatType.Agility, aglPanel);
        panelDict.Add(IntStatType.Healing, healPanel);
        panelDict.Add(IntStatType.CritRate, critPanel);
        panelDict.Add(IntStatType.EvadeRate, evadePanel);
    }

    public void DisplayStats(int currentHP, int currentMP, Dictionary<IntStatType, int> stats)
    {
        foreach (KeyValuePair<IntStatType, StatPanelPreview> entry in panelDict)
        {
            bool isPercentage = false;
            if (entry.Key == IntStatType.CritRate || entry.Key == IntStatType.EvadeRate)
            {
                isPercentage = true;
            }
            entry.Value.SetValue(stats[entry.Key], isPercentage);
        }
        hpPanel.SetCurrentValue(currentHP);
        mpPanel.SetCurrentValue(currentMP);

        HidePreview();
    }

    public void DisplayPreview(StatComparisonData statComparisonData)
    {
        foreach (KeyValuePair<IntStatType, StatPanelPreview> entry in panelDict)
        {
            int originalValue = statComparisonData.OriginalStats[entry.Key];

            int previewValue = 0;
            if (statComparisonData.ModifiedStats.ContainsKey(entry.Key))
            {
                previewValue = statComparisonData.ModifiedStats[entry.Key];
            }

            bool isPercentage = false;
            if (entry.Key == IntStatType.CritRate || entry.Key == IntStatType.EvadeRate)
            {
                isPercentage = true;
            }

            entry.Value.DisplayPreview(originalValue, previewValue, isPercentage);
        }
    }

    public void HidePreview()
    {
        foreach (KeyValuePair<IntStatType, StatPanelPreview> entry in panelDict)
        {
            entry.Value.HidePreview();
        }
    }

    public void Hide()
    {
        //if (content.activeInHierarchy)
        //{
        //    content.SetActive(false);
        //}
    }
}
