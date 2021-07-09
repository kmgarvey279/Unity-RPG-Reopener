using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu(fileName = "New Signal Sender (String)", menuName = "SignalSender/String")]
public class SignalSenderString : ScriptableObject
{
    private List<SignalListenerString> listeners = new List <SignalListenerString>();

    public void Raise(string arg)
    {
        for(int i = listeners.Count - 1; i >= 0; i--)
        {
            listeners[i].OnSignalRaised(arg);
        }
    }

    public void RegisterListener(SignalListenerString listener)
    {
        listeners.Add(listener);
    }

    public void RemoveListener(SignalListenerString listener)
    {
        listeners.Remove(listener);
    }
}
