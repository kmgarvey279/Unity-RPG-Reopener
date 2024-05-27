using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Events;

public class SignalListenerInt : SignalListenerBase
{
    public SignalSenderInt signalSender;

    [System.Serializable]
    public class CustomUnityEvent : UnityEvent<int> {}
    public CustomUnityEvent signalEvent;

    public virtual void OnSignalRaised(int arg)
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
