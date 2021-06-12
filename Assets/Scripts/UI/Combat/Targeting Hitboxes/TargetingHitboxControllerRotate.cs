using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class TargetingHitboxControllerRotate : TargetingHitboxController 
{
    private Vector3 rotateDirection;
    [SerializeField] private Transform hitboxAxis;

    private void Update()
    {
        float moveX = Input.GetAxisRaw("Horizontal");
        float moveY = Input.GetAxisRaw("Vertical");

        if(!Mathf.Approximately(moveX, 0.0f) || !Mathf.Approximately(moveY, 0.0f))
        {
            rotateDirection = new Vector3(moveX, moveY).normalized; 
        } 
    }

    private void FixedUpdate()
    {
        float angle = Mathf.Atan2(rotateDirection.x, -rotateDirection.y) * Mathf.Rad2Deg;
        hitboxAxis.rotation = Quaternion.Euler(0, 0, angle); 
    }
}
