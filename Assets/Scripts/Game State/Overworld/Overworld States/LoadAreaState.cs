using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class LoadAreaState : OverworldState
{
    public override void OnEnter()
    {
        base.OnEnter();
        //StartCoroutine(LoadAreaCo());
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

    //public IEnumerator LoadAreaCo()
    //{
    //    yield return StartCoroutine(overworldManager.LoadAreaCo());

    //    stateMachine.ChangeState((int)OverworldStateType.FreeMove);
    //}
}
