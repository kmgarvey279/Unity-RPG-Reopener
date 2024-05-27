using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;
using UnityEngine.EventSystems;
using UnityEngine.UI;

public class ScrollableListSlotSkill : ScrollableListSlot
{
    [field: SerializeField] public Action Action { get; private set; }
    public int MPCost { get; private set; }

    [SerializeField] private TextMeshProUGUI nameText;
    [SerializeField] private TextMeshProUGUI costValueText;
    [SerializeField] private TextMeshProUGUI costTypeText;
    [SerializeField] private Color disabledColor;
    [SerializeField] private Image icon;
    [SerializeField] private SignalSenderGO onSelectSkill;
    [SerializeField] private SignalSenderGO onClickSkill;
    private bool isEnabled = true;

    public override void OnSelect(BaseEventData eventData)
    {
        base.OnSelect(eventData);
        onSelectSkill.Raise(this.gameObject);
    }

    public override void OnClick()
    {
        if (isEnabled)
        {
            onClickSkill.Raise(this.gameObject);
        }
    }

    public void AssignAction(Action action, int mpCost)
    {
        Action = action;
        MPCost = mpCost;
        nameText.text = action.ActionName;

        if (action.ConsumeAllMP)
        {
            costValueText.text = "All";
        }
        else if (MPCost > 0)
        {
            costValueText.text = MPCost.ToString();
        }
        else
        {
            costValueText.text = "";
        }
        icon.sprite = action.Icon;
    }

    private void ToggleEnabled(bool _isEnabled)
    {
        isEnabled = _isEnabled;
        nameText.color = disabledColor;
        costValueText.color = Color.red;
    }

    public void CheckMP(int currentMP)
    {
        if (currentMP < MPCost)
        {
            ToggleEnabled(false);
        }
    }
}
