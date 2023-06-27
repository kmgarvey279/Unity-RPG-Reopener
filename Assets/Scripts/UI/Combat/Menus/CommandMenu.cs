using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.EventSystems;

public class CommandMenu : MonoBehaviour
{
    [Header("External References")]
    [SerializeField] protected BattleManager battleManager;
    [SerializeField] protected BattleTimeline battleTimeline;
    [Header("Components")]
    [SerializeField] protected GameObject display;
    [Header("Generic Buttons")]
    [SerializeField] protected GameObject defaultButton;
    [Header("Events")]
    [SerializeField] protected SignalSenderInt onChangeBattleState;
    
    public virtual void Display()
    {
        display.SetActive(true);
        EventSystem.current.SetSelectedGameObject(null);
        EventSystem.current.SetSelectedGameObject(defaultButton);
    }

    public virtual void Hide()
    {
        display.SetActive(false);
        EventSystem.current.SetSelectedGameObject(null);
    }
}
