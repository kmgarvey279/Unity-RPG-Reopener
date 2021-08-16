using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class MaskController : MonoBehaviour
{
    private Animator animator;

    private void Start()
    {
        animator = GetComponent<Animator>();
    }

    public void TriggerSelected()
    {
        animator.SetTrigger("Selected");
    }

    public void TriggerUnselectable()
    {
        animator.SetTrigger("Unselectable");
    }

    public void EndAnimation()
    {
        animator.SetTrigger("Default");
    }
}
