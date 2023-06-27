using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using TMPro;
using UnityEngine.EventSystems;

public class DynamicListSlotAction : DynamicListSlot
{
    [SerializeField] private SignalSenderString onUpdateDescription;
    [SerializeField] private SignalSenderGO onSelectSkillBattle;
    [Header("Action Info")]
    [SerializeField] private OutlinedText costTypeText;
    [SerializeField] private OutlinedText costAmountText;
    [SerializeField] private Color mpTextColor;
    [SerializeField] private Color mpIconColor;
    public Action Action { get; private set; }

    public void AssignAction(Action action)
    {           
        this.Action = action;
        icon.sprite = action.Icon;
        nameText.SetText(action.ActionName);
        if (action.Cost > 0)
        {
            if (action.ActionCostType == ActionCostType.HP)
            {
                costTypeText.SetText("HP");
            }
            else if (action.ActionCostType == ActionCostType.MP)
            {
                costTypeText.SetText("MP");
            }
            costAmountText.SetText(action.Cost.ToString("n0"));
        }
    }

    public override void OnSelect(BaseEventData eventData)
    {
        base.OnSelect(eventData);
        onUpdateDescription.Raise(Action.Description);
    }

    public void OnClick()
    {
        onSelectSkillBattle.Raise(this.gameObject);
    }
}
