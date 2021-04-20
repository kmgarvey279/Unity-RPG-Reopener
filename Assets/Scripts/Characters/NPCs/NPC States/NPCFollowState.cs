using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using StateMachineNamespace;
using Pathfinding;

[System.Serializable]
public class NPCFollowState : NPCState
{
//     public Transform targetFollowPoint;
//     public Transform followAxis;
//     public float followDistance;
//     public float followSpeed;


//     public override void OnEnter()
//     {
//         nextState = "";
//         targetFollowPoint = character.followPoint;
//     }

//     public override void StateUpdate()
//     {
//     }

//     public override void StateFixedUpdate()
//     {
//         Vector3 lookTemp = targetFollowPoint.position - transform.position;
//         character.ChangeLookDirection(lookTemp);

//         float angle = Mathf.Atan2(lookTemp.x, -lookTemp.y) * Mathf.Rad2Deg;
//         followAxis.rotation = Quaternion.Euler(0, 0, angle);

//         float tempX = Mathf.Round(character.lookDirection.x);
//         float tempY = Mathf.Round(character.lookDirection.y);
//         animator.SetFloat("Look X", tempX);
//         animator.SetFloat("Look Y", tempY);

//         Vector3 moveTemp =  new Vector3(0,0,0);

//  if(Vector3.Distance(transform.position, targetFollowPoint.position) > followDistance)
//         {
//             moveTemp =  Vector3.MoveTowards(transform.position, targetFollowPoint.position, followSpeed * Time.deltaTime);
//             rigidbody.MovePosition(moveTemp);
//         }
        
//         animator.SetFloat("Speed", moveTemp.sqrMagnitude);
//     }


//     public override string CheckConditions()
//     {
//         return nextState;
//     }

//     public override void OnExit()
//     {

//     }	
}
