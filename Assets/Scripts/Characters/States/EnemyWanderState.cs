using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using StateMachineNamespace;
using Pathfinding;

public class EnemyWanderState : StateMachine.State
{
    private Character character;

    public override void OnEnter()
    {
        character = GetComponentInParent<Character>();
        nextState = "";

        StartCoroutine(SetDestination());
    }

    public override void StateUpdate()
    {
        if(character.aiPath.reachedDestination)
        {
            StartCoroutine(SetDestination());
        }
    }

    public override void StateFixedUpdate()
    {
        if(!Mathf.Approximately(character.aiPath.targetDirection.x, 0.0f) || !Mathf.Approximately(character.aiPath.targetDirection.y, 0.0f))
        {
            character.animator.SetFloat("Look X", Mathf.Round(character.aiPath.targetDirection.x));
            character.animator.SetFloat("Look Y", Mathf.Round(character.aiPath.targetDirection.y));
        }
        Vector3 velocity = character.aiPath.CalculateVelocity(transform.position);
        character.animator.SetFloat("Speed", velocity.sqrMagnitude);

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

        float speedTemp = character.aiPath.maxSpeed;
        character.aiPath.maxSpeed = 0;

        Vector3 newDestination = Random.insideUnitSphere * 5;
        character.aiPath.destination = newDestination;  

        yield return new WaitForSeconds(delay);
        character.aiPath.maxSpeed = speedTemp;

    }

    private void OnTriggerEnter2D(Collider2D other)
    {
        if(other.gameObject.CompareTag("Player"))
        {
            nextState = "BattleState";
        }
    }
}
