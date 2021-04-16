using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Player : Character
{
    [SerializeField] private Transform firePoint;
    
    public override void Start()
    {
        base.Start();
    }

    public override void ChangeLookDirection(Vector3 newDirection)
    {
        base.ChangeLookDirection(newDirection);
        float angle = Mathf.Atan2(lookDirection.x, -lookDirection.y) * Mathf.Rad2Deg;
        firePoint.rotation = Quaternion.Euler(0, 0, angle);
    }
}
