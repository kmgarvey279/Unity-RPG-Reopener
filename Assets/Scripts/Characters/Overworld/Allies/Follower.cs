using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Follower : Ally
{   
    [SerializeField] public Transform partyPosition;

    [SerializeField] private float followDistance;

    public void FixedUpdate()
    {
        
        float currentSpeed = 0;

        if(isRunning)
        {
            currentSpeed = runSpeed;
        }
        else 
        {
            currentSpeed = walkSpeed;
        }

        Vector3 moveTemp = new Vector3(0,0);
        if(Vector3.Distance(transform.position, partyPosition.position) > followDistance)
        {
            moveTemp = Vector3.MoveTowards(transform.position, partyPosition.position, currentSpeed * Time.deltaTime);
            rigidbody.MovePosition(moveTemp);
        }
        animator.SetFloat("Speed", moveTemp.sqrMagnitude);
    }
}
