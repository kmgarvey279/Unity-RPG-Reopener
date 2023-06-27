using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Events;

public class SignalListenerGO : MonoBehaviour
{
    public SignalSenderGO signalSender;
    
    [System.Serializable]
    public class CustomUnityEvent : UnityEvent<GameObject> {}
    public CustomUnityEvent signalEvent;

    public virtual void OnSignalRaised(GameObject arg)
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
