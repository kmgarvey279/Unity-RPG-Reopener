using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Events;

public class SignalListenerCombatant : MonoBehaviour
{
    public SignalSenderCombatant signalSender;

    [System.Serializable]
    public class CustomUnityEvent : UnityEvent<Combatant, float> { }
    public CustomUnityEvent signalEvent;

    public virtual void OnSignalRaised(Combatant arg1, float arg2)
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
