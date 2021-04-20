using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using StateMachineNamespace;
using Pathfinding;

[System.Serializable]
public class NPCBattleState : NPCState
{
    // [SerializeField] private Targeter targeter;
    // private string animatorTrigger;
    // // [SerializeField] private PlayerParty playerParty;
    // [SerializeField] private Transform startPosition;

    // public override void OnEnter()
    // {
    //     //activate collision and pathfinding

    //     character.aiPath.canMove = true;
    //     character.setter.target = startPosition;

    //     targeter = GetComponent<Targeter>();
    //     playerParty = GetComponentInParent<PlayerParty>();

    //     // playerParty.AddTargetToAll(GetComponentInParent<Enemy>().gameObject);
    //     // foreach (GameObject target in playerParty.activeParty)
    //     // {
    //     //     targeter.AddTarget(target);         
    //     // }

    //     nextState = "";
    // }

    // public override void StateUpdate()
    // {
    // }

    // public override void StateFixedUpdate()
    // {
    // }

    // public override string CheckConditions()
    // {
    //     return nextState;
    // }

    // public override void OnExit()
    // {
    //     animator.SetTrigger("End Action");
    //     // character.actionManager.FinishAction();
    // }


    // public void OnAnimationComplete()
    // {
    //     nextState = "MoveState";
    // }

}
