using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu(fileName = "New Signal Sender (Battle Event)", menuName = "SignalSender/Battle Event")]
public class SignalSenderBattleEvent : ScriptableObject
{
    private List<SignalListenerBattleEvent> listeners = new List<SignalListenerBattleEvent>();

    public void Raise(BattleEvent arg)
    {
        for (int i = listeners.Count - 1; i >= 0; i--)
        {
            listeners[i].OnSignalRaised(arg);
        }
    }

    public void RegisterListener(SignalListenerBattleEvent listener)
    {
        listeners.Add(listener);
    }

    public void RemoveListener(SignalListenerBattleEvent listener)
    {
        listeners.Remove(listener);
    }
}

