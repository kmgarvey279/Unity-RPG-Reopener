using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using StateMachineNamespace;

public class AllyFollowState : StateMachine.State
{
    public PlayableCharacter character;

    public Transform followerAxis;
    
    public float followDistance;
    public float followSpeed;

    public override void OnEnter()
    {
        character = GetComponentInParent<PlayableCharacter>();
        nextState = "";
        character.boxCollider.enabled = false;
        character.aiPath.canMove = false;
    }

    public override void StateUpdate()
    {
    }

    public override void StateFixedUpdate()
    {
        Follow();
    }

    public override string CheckConditions()
    {
        return nextState;
    }

    public override void OnExit()
    {

    }	

    private void Follow()
    {
        Vector3 lookTemp = character.partyPosition.position - transform.position;
        character.ChangeLookDirection(lookTemp);

        float angle = Mathf.Atan2(lookTemp.x, -lookTemp.y) * Mathf.Rad2Deg;
        followerAxis.rotation = Quaternion.Euler(0, 0, angle);

        float tempX = Mathf.Round(character.lookDirection.x);
        float tempY = Mathf.Round(character.lookDirection.y);
        character.animator.SetFloat("Look X", tempX);
        character.animator.SetFloat("Look Y", tempY);

        Vector3 moveTemp =  new Vector3(0,0,0);

        if(Vector3.Distance(transform.position, character.partyPosition.position) > followDistance)
        {
            moveTemp =  Vector3.MoveTowards(transform.position, character.partyPosition.position, followSpeed * Time.deltaTime);
            character.rigidbody.MovePosition(moveTemp);
        }
        character.animator.SetFloat("Speed", moveTemp.sqrMagnitude);
    }

    public void EnterBattleState()
    {
        nextState = "BattleState";
    }
}
