using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.EventSystems;

public class BattleLogSender : MonoBehaviour, ISelectHandler
{
    [SerializeField][TextArea(1, 2)] private string text;
    [SerializeField] private SignalSenderString onUpdateBattleLog;

    public void OnSelect(BaseEventData eventData)
    {
        onUpdateBattleLog.Raise(text);
    }
}
