using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Pathfinding;

public class NPCIdleState : NPCState
{
    // public Vector3 destination;
    public AIPath aiPath;
    public string otherTag;

    public override void OnEnter()
    {
        aiPath = GetComponentInParent<AIPath>();
        StartCoroutine(SetDestination());
        
        nextState = "";
    }

    public override void StateUpdate()
    {
        if(aiPath.reachedDestination)
        {
            StartCoroutine(SetDestination());
        }
    }

    public override void StateFixedUpdate()
    {
        if(!Mathf.Approximately(aiPath.targetDirection.x, 0.0f) || !Mathf.Approximately(aiPath.targetDirection.y, 0.0f))
        {
            animator.SetFloat("Look X", Mathf.Round(aiPath.targetDirection.x));
            animator.SetFloat("Look Y", Mathf.Round(aiPath.targetDirection.y));
        }
        Vector3 velocity = aiPath.CalculateVelocity(transform.position);
        animator.SetFloat("Speed", velocity.sqrMagnitude);

    }


    public override string CheckConditions()
    {
        return nextState;
    }

    public override void OnExit()
    {

    }	

    private IEnumerator SetDestination()
    {
        float delay = Random.Range(1f, 5f);

        float speedTemp = aiPath.maxSpeed;
        aiPath.maxSpeed = 0;

        Vector3 newDestination = Random.insideUnitSphere * 5;
        aiPath.destination = newDestination;  

        yield return new WaitForSeconds(delay);
        aiPath.maxSpeed = speedTemp;

    }

    private void OnTriggerEnter2D(Collider2D other)
    {
        if(other.gameObject.CompareTag(otherTag))
        {
            nextState = "BattleState";
        }
    }
}
