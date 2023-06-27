using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[System.Serializable]
public class BattleCondition : ScriptableObject 
{
    public virtual bool CheckCondition(ActionSubevent actionSubevent)
    {
        return false;
    }
}
