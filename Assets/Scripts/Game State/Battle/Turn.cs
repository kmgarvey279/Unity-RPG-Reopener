using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System.Linq;
using UnityEngine.Rendering;
using System;

public enum TurnType
{
    Standard,
    Cast,
    Intervention,
    Paused
}

//stores information related to combatant's place in turn queue
[System.Serializable]
public class Turn
{
    private float defaultTurnCost = 200f;
    private float DelayTotal = 0;
    [field: SerializeField] public TurnType TurnType { get; private set; }
    [field: SerializeField] public Combatant Actor { get; private set; }
    [field: SerializeField] public string ActorName { get; private set; } = "";
    [field: SerializeField] public Action Action { get; private set; }
    [field: SerializeField] public List<Combatant> Targets { get; private set; } = new List<Combatant>();
    public CombatantType TargetedCombatantType { get; private set; }
    [field: SerializeField] public int Counter { get; protected set; }
    [field: SerializeField] public TurnPanel TurnPanel { get; protected set; }
    public int TempModifier { get; private set; }
    public bool IsTargeted { get; private set; } = false;
    public bool WasChanged { get; private set; } = false;
    public bool ReselectTargets { get; private set; } = false;
    [field: SerializeField] public int QueueIndex { get; private set; } = -1;

    public Turn(TurnType turnType, Combatant actor, float turnMultiplier)
    {
        TurnType = turnType;
        Actor = actor;
        if(Actor != null)
        {
            ActorName = Actor.CharacterName + " " +  Actor.CharacterLetter;
        }
        if(turnType is TurnType.Intervention)
        {
            Counter = -8888;
        }
        else
        {
            Counter = Mathf.FloorToInt((defaultTurnCost - GetSpeed()) * turnMultiplier);
        }
    }

    public void SetQueueIndex(int newIndex)
    {
        QueueIndex = newIndex;
    }

    public void SetActor(Combatant actor)
    {
        Actor = actor;
    }


    public void SetAction(Action action)
    {
        Action = action;
        if (Action != null)
        {
            TargetedCombatantType = CombatantType.Player;
            if (Action.TargetingType == TargetingType.TargetHostile && Actor.CombatantType == CombatantType.Player
                || Action.TargetingType == TargetingType.TargetFriendly && Actor.CombatantType == CombatantType.Enemy)
            {
                TargetedCombatantType = CombatantType.Enemy;
            }
        }
    }

    public void SetTargets(List<Combatant> targets)
    {
        Targets = targets;
    }

    public void SetTurnPanel(TurnPanel turnPanel)
    {
        TurnPanel = turnPanel;
    }

    public void CancelAction()
    {
        Action = null;
        Targets.Clear();
    }

    public void SetReselectTargets(bool reselectTargets)
    {
        ReselectTargets = reselectTargets;
    }


    public float GetSpeed()
    {
        if(Actor)
        {
            return Actor.Stats[StatType.Agility].GetValue();
        }
        else
        {
            return 0f;
        }
    }

    public void SetAsCurrentTurn()
    {
        Counter = -9999;
    }

    public void PauseTurn()
    {
        TurnType = TurnType.Paused;
        Counter = -7777;
    }

    public void ApplyModifier(float modifierToAdd, bool isTemp)
    {
        //cap delay at 100%
        if (modifierToAdd > 0 && DelayTotal + modifierToAdd >= 1f)
        {
            modifierToAdd = 1f - DelayTotal;
        }
        
        int amountToAdd = Mathf.FloorToInt(modifierToAdd * (defaultTurnCost - GetSpeed()));
        
        //prevent counter from going negative
        if(amountToAdd + Counter < 0) 
        {
            amountToAdd = -Counter;
        }

        Counter = Counter + amountToAdd;

        if(isTemp)
        {
            TempModifier = amountToAdd;
        }
        else if (modifierToAdd > 0) 
        {
            DelayTotal += modifierToAdd;
        }
        //if it had an effect
        if (amountToAdd != 0 && isTemp)
        {
            TurnPanel.DisplayTurnModifier(amountToAdd);
        }

        WasChanged = true;
    }

    public void RemoveTempModifier()
    {
        if(TempModifier != 0)
        {
            Counter -= TempModifier;
            TempModifier = 0;

            WasChanged = true;
        }
    }

    public void RemoveChangedState()
    {
        WasChanged = false;
    }

    public void Tick()
    {
        if(Counter > 0)
        {
            Counter--;
        }
    }

    //public void SetTargeted(bool isTargeted)
    //{
    //    IsTargeted = isTargeted;
    //}
}
