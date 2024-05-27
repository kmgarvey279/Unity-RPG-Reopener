using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Events;

public class SignalListenerBool : SignalListenerBase
{
    public SignalSenderBool signalSender;

    [System.Serializable]
    public class CustomUnityEvent : UnityEvent<bool> {}
    public CustomUnityEvent signalEvent;

    public virtual void OnSignalRaised(bool arg)
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
