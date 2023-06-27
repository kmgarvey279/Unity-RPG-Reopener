using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.EventSystems;

public class CommandMenuList : MonoBehaviour
{
    [Header("External References")]
    [SerializeField] protected BattleManager battleManager;
    [SerializeField] protected BattleTimeline battleTimeline;
    [SerializeField] protected CommandMenuMain commandMenuMain;
    [Header("Components")]
    [SerializeField] protected GameObject display;
    [SerializeField] protected SignalSenderInt onChangeBattleState;

    public virtual void DisplayList()
    {
        display.SetActive(true);
    }

    public virtual void HideList()
    {
        EventSystem.current.SetSelectedGameObject(null);
        display.SetActive(false);
    }

    public virtual void OnSelectSlot(GameObject slotObject)
    {
        HideList();
    }
}
