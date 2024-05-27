using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu(fileName = "New Signal Sender (Bool)", menuName = "SignalSender/Bool")]
public class SignalSenderBool : ScriptableObject
{
    private List<SignalListenerBool> listeners = new List <SignalListenerBool>();

    public void Raise(bool arg)
    {
        for(int i = listeners.Count - 1; i >= 0; i--)
        {
            listeners[i].OnSignalRaised(arg);
        }
    }

    public void RegisterListener(SignalListenerBool listener)
    {
        listeners.Add(listener);
    }

    public void RemoveListener(SignalListenerBool listener)
    {
        listeners.Remove(listener);
    }
}
