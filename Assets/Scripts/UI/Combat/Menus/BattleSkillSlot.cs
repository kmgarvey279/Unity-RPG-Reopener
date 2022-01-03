using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;
using UnityEngine.UI;
 using UnityEngine.EventSystems;

public class BattleSkillSlot : MonoBehaviour, ISelectHandler, IPointerEnterHandler
{
    [Header("Skill In Slot")]
    public Action action;
    [Header("UI Display")]
    [SerializeField] private Image skillIcon;
    [SerializeField] private TextMeshProUGUI nameText;
    [SerializeField] private TextMeshProUGUI costText;
    [Header("Events")]
    [SerializeField] private SignalSenderGO onSelectBattleSkill;
    [SerializeField] private SignalSenderGO onConfirmBattleSkill;

    public void AssignSlot(Action action)
    {
        this.action = action;
        skillIcon = action.icon;
        nameText.text = action.name;
        costText.text = action.mpCost.ToString("n0");
    }

    public void OnSelect(BaseEventData eventData)
    {
        onSelectBattleSkill.Raise(this.gameObject);
    }

    public void OnPointerEnter(PointerEventData eventData)
    {
        EventSystem.current.SetSelectedGameObject(null);
        EventSystem.current.SetSelectedGameObject(this.gameObject);
    }

    public void OnClick()
    {
        onConfirmBattleSkill.Raise(this.gameObject);
    }
}
