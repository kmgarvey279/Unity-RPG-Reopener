using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu(fileName = "New Battle Condition", menuName = "Battle Conditions/Hit Count")]
public class BattleConditionHitCount : BattleCondition
{
    [SerializeField] private bool orGreaterThen = false;
    public override bool CheckCondition(Combatant combatantToCheck, float value, ActionSummary actionSummary = null)
    {
        int intValue = Mathf.FloorToInt(value);

        if (actionSummary != null)
        {
            if (orGreaterThen)
            {
                if(actionSummary.Hits >= intValue)
                {
                    return true;
                }
                return false;
            }
            else
            {
                if (actionSummary.Hits == intValue)
                {
                    Debug.Log("Hit count is true");
                    return true;
                }
                Debug.Log("Hit count is false");
                return false;
            }
        }
        Debug.Log("no action summary!");
        return false;
    }
}
