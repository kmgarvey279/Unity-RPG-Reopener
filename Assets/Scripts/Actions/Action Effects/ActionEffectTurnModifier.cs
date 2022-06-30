using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[System.Serializable]
public class ActionEffectTurnModifier : ActionEffect
{
    public float turnModifier;
    public override void ApplyEffect(ActionEvent actionEvent)
    {
        // actionEvent.targets[0].AddStatusEffect(turnEffect);
        actionEvent.targets[0].ApplyTurnModifier(turnModifier);
        actionEvent.turnModifierApplied = true;
    }
}
