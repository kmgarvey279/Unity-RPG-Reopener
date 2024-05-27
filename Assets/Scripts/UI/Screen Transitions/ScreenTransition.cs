using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class ScreenTransition : MonoBehaviour
{
    [SerializeField] private Animator animator;
    [SerializeField] private Image image;

    private void OnEnable()
    {
        image.enabled = true;
    }

    public void TriggerOff()
    {
        animator.SetTrigger("Off");
    }

    public void TriggerOn()
    {
        animator.SetTrigger("On");
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
