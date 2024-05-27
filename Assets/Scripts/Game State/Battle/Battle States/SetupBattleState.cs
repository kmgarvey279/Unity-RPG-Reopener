using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[System.Serializable]
public class SetupBattleState : BattleState
{
    public override void OnEnter()
    {
        base.OnEnter();
        Debug.Log("Entering Setup Battle State");
    }

    public override void StateUpdate()
    {

    }

    public override void StateFixedUpdate()
    {

    }

    public override void OnExit()
    {
        base.OnExit();
    }
}
