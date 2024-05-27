using System.Collections;
using System.Collections.Generic;
using UnityEngine;

//public enum TurnType
//{
//    Standard,
//    Cast,
//    Intervention,
//    Paused
//}

//stores information related to combatant's place in turn queue
[System.Serializable]
public class Turn
{
    private float defaultTurnCost = 200f;
    //private float DelayTotal = 0;
    private int oneTurnModifierTotal;

    //[field: SerializeField] public TurnType TurnType { get; private set; }
    [field: SerializeField] public Combatant Actor { get; private set; }
    [field: SerializeField] public string ActorName { get; private set; } = "";
    [field: SerializeField] public Action Action { get; private set; }
    [field: SerializeField] public List<Combatant> Targets { get; private set; } = new List<Combatant>();
    public CombatantType TargetedCombatantType { get; private set; }
    [field: SerializeField] public int Counter { get; protected set; }
    [field: SerializeField] public TurnPanel TurnPanel { get; protected set; }
    public int TempModifier { get; private set; }
    public bool IsIntervention { get; private set; } = false;
    public bool IsTargeted { get; private set; } = false;
    public bool WasChanged { get; private set; } = false;
    public bool WasSwappedOut { get; private set; } = false;
    public bool SwappedThisTurn { get; private set; } = false;
    public Item ItemToUse { get; private set; }
    [field: SerializeField] public int QueueIndex { get; private set; } = -1;

    public Turn(Combatant actor, float turnMultiplier, bool isIntervention)
    {
        Actor = actor;
        
        if (Actor != null)
        {
            ActorName = Actor.CharacterName + " " +  Actor.CharacterLetter;
        }
        if (isIntervention)
        {
            IsIntervention = true;
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

    public void SetItemToUse(Item item)
    {
        ItemToUse = item;
    }

    public void SetTurnPanel(TurnPanel turnPanel)
    {
        TurnPanel = turnPanel;
    }

    public void CancelAction()
    {
        Action = null;
        ItemToUse = null;
        Targets.Clear();
    }

    public float GetSpeed()
    {
        if (Actor)
        {
            return Actor.Stats[IntStatType.Agility];
        }
        else
        {
            return 0f;
        }
    }

    public void SetAsCurrentTurn()
    {
        Counter = -9999;
        TurnPanel.SetAsCurrentTurn();
    }

    //public void PauseTurn()
    //{
    //    TurnType = TurnType.Paused;
    //    Counter = -7777;
    //}

    public void ApplyModifier(float modifierToAdd, bool isTemp, bool appliedToThisTurnOnly)
    {
        //cap delay at 100%
        //if (modifierToAdd > 0 && DelayTotal + modifierToAdd >= 1f)
        //{
        //    modifierToAdd = 1f - DelayTotal;
        //}
        
        int amountToAdd = Mathf.RoundToInt(modifierToAdd * (defaultTurnCost - GetSpeed()));
        
        //prevent counter from going negative
        //if(amountToAdd + Counter < 0) 
        //{
        //    amountToAdd = -Counter;
        //}

        Counter = Counter + amountToAdd;

        if (isTemp)
        {
            TempModifier += amountToAdd;
        }
        else if (appliedToThisTurnOnly)
        {
            oneTurnModifierTotal += amountToAdd;
        }
        //else if (modifierToAdd > 0) 
        //{
        //    DelayTotal += modifierToAdd;
        //}
        if (amountToAdd != 0)
        {
            WasChanged = true;
        }
    }

    public void RemoveTempModifier()
    {
        if (TempModifier != 0)
        {
            Counter -= TempModifier;
            TempModifier = 0;

            WasChanged = true;
        }
    }

    public void RemoveOneTurnModifiers()
    {
        if (oneTurnModifierTotal != 0)
        {
            Counter -= oneTurnModifierTotal;
            oneTurnModifierTotal = 0;
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

    public void OnSwapOut()
    {
        WasChanged = true;
        WasSwappedOut = true;
    }

    public void RemoveSwappedState()
    { 
        WasSwappedOut = false;
    }

    public void OnSwapIn()
    {
        SwappedThisTurn = true;
    }

    //public void SetTargeted(bool isTargeted)
    //{
    //    IsTargeted = isTargeted;
    //}
}
