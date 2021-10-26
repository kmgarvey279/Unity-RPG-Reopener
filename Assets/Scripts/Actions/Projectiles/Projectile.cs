using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using DG.Tweening;
using System;

public class Projectile : MonoBehaviour
{
    private bool moving;
    private Vector3 target;
    private float moveSpeed = 5f;
    [SerializeField] private SignalSender onProjectileEnd;

    private void Update()
    {
        if(moving)
        {
            if(Vector3.Distance(transform.position, target) < 0.0001f)
            {
                onProjectileEnd.Raise();
                Destroy(this.gameObject);
            }
            else
            {
                float step = moveSpeed * Time.deltaTime; 
                transform.position = Vector3.MoveTowards(transform.position, target, step);   
            }
        }
    }

    public void Move(Vector3 target)
    {
        this.target = target;
        moving = true;
    }
}
