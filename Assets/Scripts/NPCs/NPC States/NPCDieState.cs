using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using StateMachineNamespace;

[System.Serializable]
public class NPCDieState : NPCState
{
    private float destroyDelay = 0.4f;

    public override void OnEnter()
    {
        character.animator.SetBool("Die", true);
        StartCoroutine(DieCo());
    }

    public override void OnExit()
    {

    }

    public IEnumerator DieCo()
    {
        yield return new WaitForSeconds(destroyDelay);
        Destroy(this.gameObject);
    }
}
