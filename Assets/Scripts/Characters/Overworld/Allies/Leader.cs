using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Leader : Ally
{
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
        rigidbody.velocity = new Vector3(moveDirection.x * currentSpeed, moveDirection.y * currentSpeed);

        //update animation
        animator.SetFloat("Speed", rigidbody.velocity.sqrMagnitude);
    }
}
