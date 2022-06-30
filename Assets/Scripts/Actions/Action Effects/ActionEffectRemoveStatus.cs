using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[System.Serializable]
public class ActionEffectRemoveStatus : ActionEffect
{
    [SerializeField] private bool removeAll = false;
    [SerializeField] private StatusEffectType statusEffectType;
    [SerializeField] private StatusEffectSO statusEffectSO;

    public virtual void ApplyEffect(ActionEvent actionEvent)
    {
        for(int i = actionEvent.targets[0].statusEffectInstances.Count - 1; i >= 0; i--)
        {
            StatusEffectInstance effectInstance = actionEvent.targets[0].statusEffectInstances[i];
            if(removeAll && statusEffectType == effectInstance.statusEffectSO.statusEffectType || statusEffectSO == effectInstance.statusEffectSO)
            {
                actionEvent.targets[0].RemoveStatusEffect(effectInstance);
            }
        }
    }
}
