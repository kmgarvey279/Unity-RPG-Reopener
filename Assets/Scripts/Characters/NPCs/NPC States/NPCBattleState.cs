using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using StateMachineNamespace;

[System.Serializable]
public class NPCBattleState : NPCMoveState
{
    [SerializeField] private Targeter targeter;
    private string animatorTrigger;
    [SerializeField] private PlayerParty playerParty;

    public override void OnEnter()
    {
        targeter = GetComponent<Targeter>();
        playerParty = FindObjectOfType<PlayerParty>();

        playerParty.AddTargetToAll(GetComponentInParent<Enemy>().gameObject);
        foreach (GameObject target in playerParty.activeParty)
        {
            targeter.AddTarget(target);         
        }

        nextState = "";
    }

    public override void StateUpdate()
    {
    }

    public override void StateFixedUpdate()
    {
    }

    public override string CheckConditions()
    {
        return nextState;
    }

    public override void OnExit()
    {
        animator.SetTrigger("End Action");
        // character.actionManager.FinishAction();
    }


    public void OnAnimationComplete()
    {
        nextState = "MoveState";
    }

}
