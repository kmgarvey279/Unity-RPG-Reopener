using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Attack : Action
{
    public float attackRange;
    public float attackMomentum;
    
    public override bool CheckConditions(Vector3 userLocation, Vector3 userDirection, string targetTag)
    {
        RaycastHit2D hit = Physics2D.Raycast(userLocation, userDirection, attackRange, ~ignoreLayer);
        Debug.DrawRay(userLocation, userDirection * attackRange, Color.blue, 3);
        if(hit.collider != null && hit.collider.gameObject.CompareTag(targetTag))
        {
            return true;
        }
        else
        {
            return false;
        }
    } 

    public override void TakeAction(Vector3 userDirection)
    {
        myAnimator.SetTrigger(animatorTrigger);
        Vector3 momentumVector = new Vector3(userDirection.x * attackMomentum, userDirection.y * attackMomentum);
        myRB.AddForce(momentumVector);
    }
}
