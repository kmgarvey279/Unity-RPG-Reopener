using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class ScreenTransition : MonoBehaviour
{
    private Animator animator;

    private void Start()
    {
        animator = GetComponent<Animator>();
    }

    public void TriggerFadeOut()
    {
        animator.SetTrigger("Fade Out");
    }

    public void TriggerFadeIn()
    {
        animator.SetTrigger("Fade In");
    }
}
