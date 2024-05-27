using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[System.Serializable]
public class ActionEvent
{
    [field: SerializeField] public Action Action { get; private set; }
    [field: SerializeField] public Combatant Actor { get; private set; }
    [field: SerializeField] public List<ActionSubevent> ActionSubevents { get; private set; } = new List<ActionSubevent>();
    public bool IsIntervention { get; private set; }

    public ActionEvent(Combatant _actor, Action _action, List<Combatant> _targets, bool _isIntervention)
    {
        Actor = _actor;
        Action = _action;
        IsIntervention = _isIntervention;

        foreach (Combatant target in _targets)
        {
            ActionSubevent actionSubevent = new ActionSubevent(Action, Actor, target, IsIntervention);
            ActionSubevents.Add(actionSubevent);
        }
    }

    public void OverrideTargets(Combatant targetToAdd, List<Combatant> targetsToRemove, float damageMultiplier)
    {
        damageMultiplier *= targetsToRemove.Count;
        bool useExistingSubevent = false;

        for (int i = ActionSubevents.Count - 1; i >= 0; i--)
        {
            if (targetToAdd == ActionSubevents[i].Target)
            {
                damageMultiplier += 1f;
                useExistingSubevent = true;

                ActionSubevents[i].ApplyPreemptiveEventMultiplier(damageMultiplier);
            }

            if (targetsToRemove.Contains(ActionSubevents[i].Target))
            {
                ActionSubevents.RemoveAt(i);
            }
        }

        if (!useExistingSubevent)
        {
            ActionSubevent newActionSubevent = new ActionSubevent(Action, Actor, targetToAdd, IsIntervention);
            newActionSubevent.ApplyPreemptiveEventMultiplier(damageMultiplier);
            ActionSubevents.Add(newActionSubevent);
        }
    }

    public int GetCumHealthEffect()
    {
        int cumHealthEffect = 0;
        foreach (ActionSubevent actionSubevent in ActionSubevents)
        {
            cumHealthEffect += actionSubevent.ActionSummary.CumHealthEffect;
        }
        return cumHealthEffect;
    }
}