using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class NPCMoveManager : MoveManager
{
    public override void HandleMoveLogic()
    {     
        if(character.targeter.currentTarget != null)
        {
            Vector3 targetLocation = character.targeter.currentTarget.transform.position;
            
            Vector3 lookTemp = targetLocation - transform.position;
            lookTemp.Normalize();
            character.ChangeLookDirection(lookTemp);

            Vector3 moveTemp = Vector3.MoveTowards(transform.position, targetLocation, character.characterInfo.moveSpeed.GetValue() * Time.deltaTime);
            character.rigidbody.MovePosition(moveTemp);
            moveTemp.Normalize();
            moveDirection = moveTemp;
        } 
    }

    // public void SetTarget()
    // {
    //     targets = GameObject.FindGameObjectsWithTag(otherTag);      
    //     distanceFromTarget = Vector3.Distance(transform.position, target.position);
    //     activeTarget =  
        //     targetsInRange = getObjects.GetObjectList();
        // if(targetsInRange.Count > 0)
        // {
        //     Transform target = FindNearestTarget();
        //     lightningDirection = target.position - firePoint.position;
        // } 
    // }

    //     public void OnDrawGizmos()
    // {
    //     Gizmos.DrawLine(transform.position, target.position);
    //     Gizmos.color = Color.yellow;
    //     Gizmos.DrawWireSphere(transform.position, enemyStats.chaseRadius);
    //     Gizmos.color = Color.red;
    //     Gizmos.DrawWireSphere(transform.position, enemyStats.attackRadius);
    //     Gizmos.color = Color.magenta;
    //     Gizmos.DrawWireSphere(transform.position, enemyStats.personalSpaceRadius);
    // }
}
