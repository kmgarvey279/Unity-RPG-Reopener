using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Events;

public class SignalListenerString : SignalListenerBase
{
    public SignalSenderString signalSender;

    [System.Serializable]
    public class CustomUnityEvent : UnityEvent<string> {}
    public CustomUnityEvent signalEvent;

    public virtual void OnSignalRaised(string arg)
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
