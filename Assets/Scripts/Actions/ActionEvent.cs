using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[System.Serializable]
public class ActionEvent
{
    [field: SerializeField] public Action Action { get; private set; }
    [field: SerializeField] public Combatant Actor { get; private set; }
    [field: SerializeField] public List<ActionSubevent> ActionSubevents { get; private set; } = new List<ActionSubevent>();
    private float chainMultiplier = 1f;

    public ActionEvent(Combatant _actor, Action _action, List<Combatant> _targets, float _chainMultiplier)
    {
        Actor = _actor;
        Action = _action;
        chainMultiplier = _chainMultiplier;
        foreach (Combatant target in _targets)
        {
            ActionSubevent actionSubevent = new ActionSubevent(Action, Actor, target, chainMultiplier);
            ActionSubevents.Add(actionSubevent);
        }
    }

    //public void SetCombatantType()
    //{
    //    TargetedCombatantType = CombatantType.Player;
    //    if(Action.TargetingType == TargetingType.TargetHostile && Actor.CombatantType == CombatantType.Player
    //        || Action.TargetingType == TargetingType.TargetFriendly && Actor.CombatantType == CombatantType.Enemy)
    //    {
    //        TargetedCombatantType = CombatantType.Enemy;
    //    }
    //}

    //public int GetHealthEffectTotal()
    //{
    //    int totalEffect = 0;
    //    foreach (ActionSubevent actionSubevent in ActionSubevents)
    //    {
    //        totalEffect += actionSubevent.HealthEffectTotal;
    //    }
    //    return totalEffect;
    //}

    public ActionSummary GetActorActionSummary()
    {
        ActionSummary actionSummary = new ActionSummary(Action);
        foreach (ActionSubevent actionSubevent in ActionSubevents)
        {
            foreach (KeyValuePair<ActionSummaryValue, bool> entry in actionSubevent.ActionSummary.Values)
            {
                if (entry.Value)
                {
                    actionSummary.SetValueAsTrue(entry.Key);
                }
            }
            actionSummary.UpdateCumHealthEffect(actionSubevent.ActionSummary.CumHealthEffect);
        }
        return actionSummary;
    }
}