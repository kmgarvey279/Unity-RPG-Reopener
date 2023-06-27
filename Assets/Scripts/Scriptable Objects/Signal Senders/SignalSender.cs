using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu(fileName = "New Signal Sender (no argument)", menuName = "SignalSender/No Arg")]
public class SignalSender : ScriptableObject
{
    private List<SignalListener> listeners = new List <SignalListener>();

    public void Raise()
    {
        for(int i = listeners.Count - 1; i >= 0; i--)
        {
            listeners[i].OnSignalRaised();
        }
    }

    public void RegisterListener(SignalListener listener)
    {
        listeners.Add(listener);
    }

    public void RemoveListener(SignalListener listener)
    {
        listeners.Remove(listener);
    }
}
