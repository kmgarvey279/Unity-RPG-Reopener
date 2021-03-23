using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerMoveManager : MoveManager
{
    [SerializeField] private Transform firePoint;

    public override void HandleMoveLogic()
    {            
        //check first axis (move)
        float moveX = Input.GetAxisRaw("Horizontal");
        float moveY = Input.GetAxisRaw("Vertical");
        moveDirection = new Vector3(moveX, moveY).normalized;
        //check second axis (look/aim)
        float lookX = Input.GetAxisRaw("Horizontal Look");
        float lookY = Input.GetAxisRaw("Vertical Look");
        //if look/aim is being held down, use to define look direction, otherwise just use move direction
        if(!Mathf.Approximately(lookX, 0.0f) || !Mathf.Approximately(lookY, 0.0f))
        {
            lookDirection = new Vector3(lookX, lookY).normalized;    
        }
        else if(!Mathf.Approximately(moveX, 0.0f) || !Mathf.Approximately(moveY, 0.0f))
        {
            lookDirection = moveDirection;
        }
        //move line of fire
        if(!Mathf.Approximately(lookDirection.x, 0.0f) || !Mathf.Approximately(lookDirection.y, 0.0f))
        {
            float angle = Mathf.Atan2(lookDirection.x, -lookDirection.y) * Mathf.Rad2Deg;
            firePoint.rotation = Quaternion.Euler(0, 0, angle);
        }
    }
}
