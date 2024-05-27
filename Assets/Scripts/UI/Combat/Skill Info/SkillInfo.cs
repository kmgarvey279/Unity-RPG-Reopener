using System.Collections;
using System.Collections.Generic;
using System.Linq;
using TMPro;
using UnityEngine;

public class SkillInfo : MonoBehaviour
{
    [Header("Summary Text")]
    [SerializeField] private TextMeshProUGUI actionNameValue;
    [SerializeField] private TextMeshProUGUI actionTypeValue;
    [SerializeField] private Color physicalColor;
    [SerializeField] private Color fireColor;
    [SerializeField] private Color iceColor;
    [SerializeField] private Color electricColor;
    [SerializeField] private Color darkColor;
    [SerializeField] private Color lightColor;
    private Dictionary<ElementalProperty, Color> elementalColors;
    [SerializeField] private Color healColor;
    [SerializeField] private Color buffColor;
    [SerializeField] private Color debuffColor;
    [SerializeField] private Color miscColor;
    [SerializeField] private TextMeshProUGUI targetTypeValue;
    [SerializeField] private GameObject actionPowerDisplay;
    [SerializeField] private TextMeshProUGUI actionPowerValue;
    [SerializeField] private GameObject hitsDisplay;
    [SerializeField] private TextMeshProUGUI hitsValue;
    [SerializeField] private GameObject accDisplay;
    [SerializeField] private TextMeshProUGUI accValue;
    [SerializeField] private TextBox textBox;

    private void Awake()
    {
        elementalColors = new Dictionary<ElementalProperty, Color>();
        elementalColors.Add(ElementalProperty.Slash, physicalColor);
        elementalColors.Add(ElementalProperty.Strike, physicalColor);
        elementalColors.Add(ElementalProperty.Pierce, physicalColor);
        elementalColors.Add(ElementalProperty.Fire, fireColor);
        elementalColors.Add(ElementalProperty.Ice, iceColor);
        elementalColors.Add(ElementalProperty.Electric, electricColor);
        elementalColors.Add(ElementalProperty.Dark, darkColor);
        elementalColors.Add(ElementalProperty.None, miscColor);
    }

    public void DisplayAction(Action action)
    {
        Clear();

        textBox.SetText(action.Description, action.SecondaryDescription);
        actionNameValue.SetText(action.ActionName);

        //type
        switch (action.ActionType)
        {
            case ActionType.Attack:
                if (action is Attack)
                {
                    Attack attack = (Attack)action;
                    if (attack.ElementalProperty == ElementalProperty.None)
                    {
                        actionTypeValue.SetText("Non-Elemental");
                    }
                    else
                    {
                        actionTypeValue.SetText(attack.ElementalProperty.ToString());
                    }
                    actionTypeValue.color = elementalColors[attack.ElementalProperty];
                    actionPowerValue.text = "x" + attack.Power.ToString("F1");
                    hitsValue.text = "x" + attack.HitCount.ToString();
                    accValue.text = attack.HitRate.ToString() + "%";
                }
                break;
            case ActionType.Heal:
                if (action is Heal)
                {
                    Heal heal = (Heal)action;
                    actionTypeValue.SetText("Heal");
                    actionTypeValue.color = healColor;
                    actionPowerValue.text = "x" + heal.Power.ToString("F1");
                }
                break;
            case ActionType.ApplyBuff:
            case ActionType.RemoveDebuff:
                actionTypeValue.SetText("Buff");
                actionTypeValue.color = buffColor;
                break;
            case ActionType.ApplyDebuff:
            case ActionType.RemoveBuff:
                actionTypeValue.SetText("Debuff");
                actionTypeValue.color = debuffColor;
                break;
            default:
                actionTypeValue.SetText("Other");
                actionTypeValue.color = miscColor;
                break;
        }
        ////targeting
        string targetText = "";
        switch (action.AOEType)
        {
            case AOEType.Single:
                if (action.TargetingType == TargetingType.TargetSelf)
                {
                    targetText = "(Self)";
                }
                else if (action.TargetingType == TargetingType.TargetAllies)
                {
                    targetText = "(Ally)";
                }
                else
                {
                    targetText = "(One)";
                }
                break;
            case AOEType.Row:
                targetText = "(Row)";
                break;
            case AOEType.All:
                if (action.TargetingType == TargetingType.TargetAllies)
                {
                    targetText = "(Allies)";
                }
                else
                {
                    targetText = "(All)";
                }
                break;
            case AOEType.Random:
                targetText = "(Random)";
                break;
            default:
                break;
        }
        //if (action.IsBackAttack)
        //{
        //    targetText += " (Back)";
        //}
        //else if (action.IsMelee)
        //{
        //    targetText += " (Front)";
        //}
        targetTypeValue.SetText(targetText);
    }

    public void Clear()
    {
        Debug.Log("Clear skill info");
        actionNameValue.text = "------";
        actionTypeValue.text = "";

        actionPowerValue.text = "-";
        hitsValue.text = "-";
        accValue.text = "-";


        targetTypeValue.SetText("");

        textBox.Clear();
    }
}

