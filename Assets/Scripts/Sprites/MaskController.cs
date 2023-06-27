using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class MaskController : MonoBehaviour
{
    private SpriteRenderer mask; 
    private Animator animator;


    private void Start()
    {
        mask = GetComponent<SpriteRenderer>();
        animator = GetComponent<Animator>();
    }

    public void ApplyTint(Color color, bool isFlashing)
    {
        mask.color = color;
        if(isFlashing)
        {
            animator.SetTrigger("FlashingTint");
        } 
        else
        {
            animator.SetTrigger("SolidTint");
        }
        mask.enabled = true;
    }

    public void RemoveTint()
    {
        mask.enabled = false;
    }
}
