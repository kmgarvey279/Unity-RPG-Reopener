using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Condition : ScriptableObject 
{
    public virtual bool CheckCondition(GameObject user, GameObject target, float range)
    {
        return false;
    }
}
