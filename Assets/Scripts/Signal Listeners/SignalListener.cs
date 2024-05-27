using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Events;

public abstract class SignalListenerBase : MonoBehaviour
{

}

public class SignalListener : SignalListenerBase
{
    public SignalSender signalSender;
    public UnityEvent signalEvent;

    public virtual void OnSignalRaised()
    {
        signalEvent.Invoke();
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
