using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using StateMachineNamespace;

[System.Serializable]
public class PlayerAttackState : PlayerState
{
    [Header("Attack Properties")]
    public float attackDuration;
    public float attackMomentum;
    [Header("Combos")]
    bool willCombo = false;
    //current attack in combo chain
    private int attackNum = 0;
    //amount of time to follow-up  
    // public float comboWindow;
    // private float comboTimer;
    // public float snapRadiusMin;
    // public float snapRadiusMax;

    public override void OnEnter()
    {
        nextState = "";
        animator.SetBool("Attacking", true);
        StartCoroutine(AttackCo());
    }

    public override void StateUpdate()
    {
        HandleInputs();
    }

    public override string CheckConditions()
    {
        return nextState;
    }

    public override void OnExit()
    {
        animator.SetBool("Attacking", false);
    }

    private void HandleInputs()
    {
        if (Input.GetButtonDown("Light Attack")) 
        {
            if(attackNum < 2) 
            {
                willCombo = true;
            }
        }
    }

    private IEnumerator AttackCo()
    {
        rigidbody.velocity = new Vector3(animator.GetFloat("Horizontal") * attackMomentum, animator.GetFloat("Vertical") * attackMomentum, 0);
        yield return new WaitForSeconds(attackDuration);
        if(willCombo)
        {
            willCombo = false;
            attackNum++;
            animator.SetTrigger("Combo Trigger");
            StartCoroutine(AttackCo());
        } 
        else 
        {
            attackNum = 0;
            nextState = "MoveState";
        }
    }

    private void SnapToEnemy()
    {

    }
}

