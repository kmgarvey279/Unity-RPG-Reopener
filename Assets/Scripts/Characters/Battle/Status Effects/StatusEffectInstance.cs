using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UIElements;

[System.Serializable]
public class StatusEffectInstance
{
    public StatusEffect StatusEffect { get; private set; }
    public int Counter { get; private set; } = 0;
    public int Potency { get; private set; } = 0;

    public StatusEffectInstance(StatusEffect _statusEffect, int _potency)
    {
        StatusEffect = _statusEffect;
        Potency = _potency;
        Counter = _statusEffect.CounterApply;
    }

    public void Tick()
    {
        if (StatusEffect.StatusCounterType == StatusCounterType.Turns) 
        { 
            Counter--;
        }
    }

    public void ModifyStacks(int amount)
    {
        Counter += amount;
    }
}
