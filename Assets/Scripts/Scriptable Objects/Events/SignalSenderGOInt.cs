using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu(fileName = "New Signal Sender (GameObject/int)", menuName = "SignalSender/GOInt")]
public class SignalSenderGOInt : ScriptableObject
{
    private List<SignalListenerGOInt> listeners = new List <SignalListenerGOInt>();

    public void Raise(GameObject arg1, int arg2)
    {
        for(int i = listeners.Count - 1; i >= 0; i--)
        {
            listeners[i].OnSignalRaised(arg1, arg2);
        }
    }

    public void RegisterListener(SignalListenerGOInt listener)
    {
        listeners.Add(listener);
    }

    public void RemoveListener(SignalListenerGOInt listener)
    {
        listeners.Remove(listener);
    }
}
