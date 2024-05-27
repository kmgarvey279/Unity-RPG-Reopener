using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Events;

public class SignalListenerFloat : SignalListenerBase
{
    public SignalSenderFloat signalSender;
    
    [System.Serializable]
    public class CustomUnityEvent : UnityEvent<float> {}
    public CustomUnityEvent signalEvent;

    public virtual void OnSignalRaised(float arg)
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
