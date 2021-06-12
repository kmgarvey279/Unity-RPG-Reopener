using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class TargetingHitboxController : MonoBehaviour
{
    public Transform target;
    private Vector3 moveDirection;

    public void SetMaxDistance(float maxDistance)
    {
        DistanceJoint2D joint = target.GetComponent<DistanceJoint2D>();
        joint.distance = maxDistance; 
    }

    private void Update()
    {
        float moveX = Input.GetAxisRaw("Horizontal");
        float moveY = Input.GetAxisRaw("Vertical");
        moveDirection = new Vector3(moveX, moveY);
    }

    private void FixedUpdate()
    {
        target.GetComponent<Rigidbody2D>().velocity = new Vector3(moveDirection.x * 7f, moveDirection.y * 7f); 
    }
}
