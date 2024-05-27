using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[System.Serializable]
public class BattleCondition : ScriptableObject 
{
    public virtual bool CheckCondition(Combatant combatantToCheck, float value, ActionSummary actionSummary)
    {
        return false;
    }
}
