using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Events;

public class SignalListenerGOInt : MonoBehaviour
{
    public SignalSenderGOInt signalSender;

    [System.Serializable]
    public class CustomUnityEvent : UnityEvent<GameObject, int> {}
    public CustomUnityEvent signalEvent;

    public virtual void OnSignalRaised(GameObject arg1, int arg2)
    {
        signalEvent.Invoke(arg1, arg2);
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
