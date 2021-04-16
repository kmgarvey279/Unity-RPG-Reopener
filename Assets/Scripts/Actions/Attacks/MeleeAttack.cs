using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class MeleeAttack : Action
{
    public float momentum;

    public override bool CheckConditions(GameObject target)
    {
        bool canTakeAction = true;
        foreach (Condition condition in conditions)
        {
            if(!condition.CheckCondition(character.gameObject, target, range))
                canTakeAction = false;
        }
        return canTakeAction;
    } 

    public override void TakeAction(GameObject target)
    {
        base.TakeAction(target);
        Vector3 momentumVector = new Vector3(character.lookDirection.x * momentum, character.lookDirection.y * momentum);
        character.GetComponent<Rigidbody>().AddForce(momentumVector);
    }
}
