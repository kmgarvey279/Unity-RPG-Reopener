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
    [SerializeField] private TextMeshProUGUI costNum;
    [SerializeField] private Transform apIcons;
    [SerializeField] private GameObject apIconPrefab;
    [Header("Events")]
    [SerializeField] private SignalSenderGO onSelectBattleSkill;
    [SerializeField] private SignalSenderGO onConfirmBattleSkill;
    [SerializeField] private SignalSenderString onUpdateBattleLog;

    public void AssignSlot(Action action)
    {
        this.action = action;
        skillIcon = action.icon;
        nameText.text = action.actionName;
        if(action.mpCost > 0)
        {
            if(action.costsHP)
            {
                costText.text = "HP";
            }
            else
            {
                costText.text = "MP";
            }
            costNum.text = action.mpCost.ToString("n0");
        }
    }

    public void OnSelect(BaseEventData eventData)
    {
        // onSelectBattleSkill.Raise(this.gameObject);
        onUpdateBattleLog.Raise(action.description);
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
