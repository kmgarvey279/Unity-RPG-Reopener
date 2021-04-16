using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu(fileName = "New Signal Sender (GameObject)", menuName = "SignalSender/GameObject")]
public class SignalSenderGO : ScriptableObject
{
    private List<SignalListenerGO> listeners = new List <SignalListenerGO>();

    public void Raise(GameObject arg)
    {
        for(int i = listeners.Count - 1; i >= 0; i--)
        {
            listeners[i].OnSignalRaised(arg);
        }
    }

    public void RegisterListener(SignalListenerGO listener)
    {
        listeners.Add(listener);
    }

    public void RemoveListener(SignalListenerGO listener)
    {
        listeners.Remove(listener);
    }
}
