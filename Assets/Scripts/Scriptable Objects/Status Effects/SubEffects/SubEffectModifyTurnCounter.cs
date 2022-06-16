using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[System.Serializable]
[CreateAssetMenu(fileName = "New SubEffect", menuName = "SubEffect/ModifyTurnCounter")]
public class SubEffectModifyTurnCounter : SubEffect
{
    [SerializeField] private int amount;
    public override void TriggerEffect(Combatant combatant)
    {
        combatant.turnCounter.ChangeValue(amount);
    }
}
