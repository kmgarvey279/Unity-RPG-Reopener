using System.Collections;
using System.Collections.Generic;
using UnityEngine;

//used for any ability with finite range
public class DistanceCondition : Condition
{
    public bool useRayCast;
    public LayerMask ignoreLayer;

    public override bool CheckCondition(GameObject user, GameObject target, float range)
    {
        Character character = user.GetComponent<Character>();
        float distanceFromTarget = Vector3.Distance(target.transform.position, user.transform.position);
        if(distanceFromTarget <= range)
        {
            if(useRayCast)
            {
                RaycastHit2D hit = Physics2D.Raycast(user.transform.position, character.lookDirection, range, ~ignoreLayer);
                Debug.DrawRay(user.transform.position, character.lookDirection * range, Color.red, 3);
                if(hit.collider != null && hit.collider.gameObject.CompareTag(target.tag))
                {
                    return true;
                }
            }
            else
            {
                return true;
            }
        }
        return false;
    }
}
