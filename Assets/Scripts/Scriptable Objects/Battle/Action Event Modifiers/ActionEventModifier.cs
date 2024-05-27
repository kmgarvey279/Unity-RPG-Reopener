using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ActionEventModifier : ScriptableObject
{
    public virtual ActionEvent ModifyActionEvent(ActionEvent actionEvent, Combatant actor, List<Combatant> targets)
    {
        return actionEvent;
    }
}
