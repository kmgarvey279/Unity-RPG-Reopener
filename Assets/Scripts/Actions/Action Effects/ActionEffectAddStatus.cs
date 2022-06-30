using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[System.Serializable]
public class ActionEffectAddStatus : ActionEffect
{
    public StatusEffectSO statusEffectSO;
    public override void ApplyEffect(ActionEvent actionEvent)
    {
        ApplyStatusEffect(actionEvent);
    }

    private void ApplyStatusEffect(ActionEvent actionEvent)
    {
        actionEvent.targets[0].AddStatusEffect(statusEffectSO);
    }
}
