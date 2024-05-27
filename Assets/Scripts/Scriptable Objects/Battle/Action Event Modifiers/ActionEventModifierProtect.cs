using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu(fileName = "NewBActionEventModifier", menuName = "ActionEventModifier/Protect")]
public class ActionEventModifierProtect : ActionEventModifier
{
    [SerializeField] private float damageMultiplier = 1f;
    public override ActionEvent ModifyActionEvent(ActionEvent actionEvent, Combatant actor, List<Combatant> targets)
    {
        actionEvent.OverrideTargets(actor, targets, damageMultiplier);

        return actionEvent;
    }
}
