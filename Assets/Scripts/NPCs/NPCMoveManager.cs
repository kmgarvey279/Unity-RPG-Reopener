using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class NPCMoveManager : MoveManager
{
    [SerializeField] private GetObjectsInRange getObjects;
    private List<GameObject> targetsInRange = new List<GameObject>();
    // private NPCTargetManager targetManager;
    private BoolValue aggro;
    // private GameObject[] targets;
    private GameObject currentTarget;
    private Vector3 distanceFromTarget;
    [SerializeField] private string otherTag;

    public virtual void Start()
    {
        myRB = GetComponent<Rigidbody2D>();
        myAnimator = GetComponent<Animator>();
        lookDirection = new Vector3(0,-1,0); 
        moveDirection = new Vector3(0,0,0);
        currentTarget = GameObject.FindGameObjectWithTag(otherTag);
    }

    public override void HandleMoveLogic()
    {     
        if(currentTarget != null)
        {
            lookDirection = currentTarget.transform.position - transform.position;
            lookDirection.Normalize();
            Vector3 tempVector = Vector3.MoveTowards(transform.position, currentTarget.transform.position, moveSpeed * Time.deltaTime);
            myRB.MovePosition(tempVector);
            moveDirection = tempVector.normalized;
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
