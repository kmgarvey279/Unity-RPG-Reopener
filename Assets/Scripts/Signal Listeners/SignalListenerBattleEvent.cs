using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Events;

public class SignalListenerBattleEvent : SignalListenerBase
{
    public SignalSenderBattleEvent signalSender;

    [System.Serializable]
    public class CustomUnityEvent : UnityEvent<BattleEvent> {}
    public CustomUnityEvent signalEvent;

    public virtual void OnSignalRaised(BattleEvent arg)
    {
        signalEvent.Invoke(arg);
    }

    private void OnEnable()
    {
        signalSender.RegisterListener(this);
    }

    private void OnDisable()
    {
        signalSender.RemoveListener(this);
    }
}
