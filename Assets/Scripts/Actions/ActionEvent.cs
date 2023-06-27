using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[System.Serializable]
public class ActionEvent
{
    public Action Action { get; private set; }
    public Combatant Actor { get; private set; }
    public List<Combatant> Targets { get; private set; } = new List<Combatant>();
    public CombatantType TargetedCombatantType { get; private set; }

    public ActionEvent(Combatant actor, Action action, List<Combatant> targets)
    {
        Actor = actor;
        Action = action;
        Targets = targets;
        SetCombatantType();
    }

    public void SetCombatantType()
    {
        TargetedCombatantType = CombatantType.Player;
        if(Action.TargetingType == TargetingType.TargetHostile && Actor.CombatantType == CombatantType.Player
            || Action.TargetingType == TargetingType.TargetFriendly && Actor.CombatantType == CombatantType.Enemy)
        {
            TargetedCombatantType = CombatantType.Enemy;
        }
    }
}