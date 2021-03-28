using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu(fileName = "New Signal Sender (float)", menuName = "SignalSender/Float")]
public class SignalSenderFloat : ScriptableObject
{
    private List<SignalListenerFloat> listeners = new List <SignalListenerFloat>();

    public void Raise(float arg)
    {
        for(int i = listeners.Count - 1; i >= 0; i--)
        {
            listeners[i].OnSignalRaised(arg);
        }
    }

    public void RegisterListener(SignalListenerFloat listener)
    {
        listeners.Add(listener);
    }

    public void RemoveListener(SignalListenerFloat listener)
    {
        listeners.Remove(listener);
    }
}
