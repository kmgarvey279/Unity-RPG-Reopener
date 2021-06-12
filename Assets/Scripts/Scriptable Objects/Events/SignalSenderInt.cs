using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu(fileName = "New Signal Sender (Int)", menuName = "SignalSender/Int")]
public class SignalSenderInt : MonoBehaviour
{
    private List<SignalListenerInt> listeners = new List <SignalListenerInt>();

    public void Raise(int arg)
    {
        for(int i = listeners.Count - 1; i >= 0; i--)
        {
            listeners[i].OnSignalRaised(arg);
        }
    }

    public void RegisterListener(SignalListenerInt listener)
    {
        listeners.Add(listener);
    }

    public void RemoveListener(SignalListenerInt listener)
    {
        listeners.Remove(listener);
    }
}
