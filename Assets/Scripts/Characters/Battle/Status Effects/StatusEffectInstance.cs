using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UIElements;

[System.Serializable]
public class StatusEffectInstance
{
    [field: SerializeField] public StatusEffect StatusEffect { get; private set; }
    [field: SerializeField] public ClampInt Duration { get; private set; } = new ClampInt(0, 0, 0);
    [field: SerializeField] public ClampInt Stacks { get; private set; } = new ClampInt(0, 0, 0);
    [field: SerializeField] public int Potency { get; private set; } = 0;
    [field: SerializeField] public List<float> TurnModifiers { get; private set; } = new List<float>();

    public StatusEffectInstance(StatusEffect _statusEffect, int _potency)
    {
        StatusEffect = _statusEffect;
        Potency = _potency;
        if (StatusEffect.HasDuration)
        {
            Duration = new ClampInt(StatusEffect.DurationToApply, 0, StatusEffect.DurationMax);
        }
        if (StatusEffect.HasStacks)
        {
            Stacks = new ClampInt(StatusEffect.StacksToApply, 0, StatusEffect.StacksMax);
        }
    }

    public void Tick()
    {
        Duration.UpdateValue(Duration.CurrentValue - 1);
        
        Potency = Mathf.Clamp(Mathf.FloorToInt(Potency * StatusEffect.PotencyTurnMultiplier), 1, 9999);
    }

    public void ModifyDuration(int amount)
    {
        Duration.UpdateValue(Duration.CurrentValue + amount);
    }

    public void ModifyStacks(int amount)
    {
        Stacks.UpdateValue(Stacks.CurrentValue + amount);
    }

    public void ModifyPotency(int amount)
    {
        if (StatusEffect.CanIncreasePotency)
        {
            Potency = Mathf.Clamp(Potency + amount, 1, 9999);
        }
        else if (amount > Potency)
        {
            Potency = amount;
        }
    }

    public void OnApplyTurnModifier(float amount)
    {
        TurnModifiers.Add(amount);
    }
}
