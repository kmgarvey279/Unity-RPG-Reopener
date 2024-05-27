using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;
using static Cinemachine.DocumentationSortingAttribute;

public class PauseMenuStatDisplay: MonoBehaviour
{
    [SerializeField] private GameObject content;
    [Header("Header")]
    [SerializeField] private TextMeshProUGUI nameValue;
    [SerializeField] private TextMeshProUGUI lvlValue;
    [SerializeField] private TextMeshProUGUI requiredEXPValue;
    [Header("HP/MP")]
    [SerializeField] private StatPanel hpPanel;
    [SerializeField] private StatPanel mpPanel;
    [Header("Stats")]
    [SerializeField] private StatPanel atkPanel;
    [SerializeField] private StatPanel defPanel;
    [SerializeField] private StatPanel mAtkPanel;
    [SerializeField] private StatPanel mDefPanel;
    [SerializeField] private StatPanel healPanel;
    [SerializeField] private StatPanel aglPanel;
    [SerializeField] private StatPanel critPanel;
    [SerializeField] private StatPanel evadePanel;
    private Dictionary<IntStatType, StatPanel> panelDict = new Dictionary<IntStatType, StatPanel>();
    [Header("Elemental Resistances")]
    [SerializeField] private StatPanel fireResistValue;
    [SerializeField] private StatPanel iceResistValue;
    [SerializeField] private StatPanel electricResistValue;
    [SerializeField] private StatPanel darkResistValue;

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

    public void DisplayStats(PlayableCombatantRuntimeData runtimeData)
    {
        if (!content.activeInHierarchy)
        {
            content.SetActive(true);
        }

        nameValue.text = runtimeData.PlayableCharacterID.ToString();
        lvlValue.text = runtimeData.Level.ToString();
        int expToNext = DatabaseDirectory.Instance.GeneralConsts.EXPRequirements[runtimeData.Level];
        if (runtimeData.Level == DatabaseDirectory.Instance.GeneralConsts.LevelCap)
        {
            requiredEXPValue.text = "-";
        }
        else
        {
            //requiredEXPValue.text = Mathf.Clamp(runtimeData.EXPToNext - runtimeData.CurrentEXP, 0, runtimeData.EXPToNext).ToString();
            requiredEXPValue.text = Mathf.Clamp((expToNext - runtimeData.CurrentEXP), 1, expToNext).ToString();
        }
        
        hpPanel.SetCurrentValue(runtimeData.CurrentHP);
        mpPanel.SetCurrentValue(runtimeData.CurrentMP);

        foreach (KeyValuePair<IntStatType, StatPanel> entry in panelDict)
        {
            bool isPercentage = false;
            if (entry.Key == IntStatType.CritRate || entry.Key == IntStatType.EvadeRate)
            {
                isPercentage = true;
            }
            entry.Value.SetValue(runtimeData.GetStat(entry.Key), isPercentage);
        }
    }

    public void Hide()
    {
        if (content.activeInHierarchy)
        {
            content.SetActive(false);
        }
    }
}
