using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;
using UnityEngine.UI;
 using UnityEngine.EventSystems;

public class BattleSkillSlot : MonoBehaviour, ISelectHandler, IPointerEnterHandler
{
    [Header("Skill In Slot")]
    public Action skill;
    [Header("UI Display")]
    [SerializeField] private Image skillIcon;
    [SerializeField] private TextMeshProUGUI nameText;
    [SerializeField] private TextMeshProUGUI costText;
    [Header("Events")]
    [SerializeField] private SignalSenderGO onSelectBattleSkill;
    [SerializeField] private SignalSenderGO onClickBattleSkill;

    public void AssignSlot(Action skill)
    {
        this.skill = skill;
        skillIcon = skill.icon;
        nameText.text = skill.name;
        costText.text = skill.mpCost.ToString("n0");
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
        onClickBattleSkill.Raise(this.gameObject);
    }
}
