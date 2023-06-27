using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu(fileName = "New Signal Sender (Combatant)", menuName = "SignalSender/Combatant")]
public class SignalSenderCombatant : ScriptableObject
{
    private List<SignalListenerCombatant> listeners = new List<SignalListenerCombatant>();

    public void Raise(Combatant arg1, float arg2 = 0)
    {
        for (int i = listeners.Count - 1; i >= 0; i--)
        {
            listeners[i].OnSignalRaised(arg1, arg2);
        }
    }

    public void RegisterListener(SignalListenerCombatant listener)
    {
        listeners.Add(listener);
    }

    public void RemoveListener(SignalListenerCombatant listener)
    {
        listeners.Remove(listener);
    }
}
