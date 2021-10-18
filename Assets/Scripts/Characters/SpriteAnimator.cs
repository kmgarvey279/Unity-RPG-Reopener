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

    public void ReturnToIdleState()
    {
        animator.SetTrigger("Idle");
    }

    public void TriggerActionEffect()
    {
        onTriggerActionEffect.Raise();
    }
}
