using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SpriteAnimator : MonoBehaviour
{
    private Animator animator;
    [SerializeField] private SignalSender onTriggerActionEffect;

    private void Start()
    {
        animator = GetComponent<Animator>();
    }

    // public void ChangeStateTrigger(string parameter)
    // {
    //     animator.SetTrigger(parameter);
    // }

    // public void ChangeStateBool(string parameter, bool newBool)
    // {
    //     animator.SetTrigger(parameter, newBool);
    // }

    public void ReturnToIdleState()
    {
        Debug.Log("idle");
        animator.SetTrigger("Idle");
    }

    public void TriggerActionEffect()
    {
        onTriggerActionEffect.Raise();
    }
}
