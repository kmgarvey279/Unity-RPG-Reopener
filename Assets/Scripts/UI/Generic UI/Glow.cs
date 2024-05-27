using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Glow : MonoBehaviour
{
    private Animator animator;

    private void OnEnable()
    {
        animator = GetComponent<Animator>();
    }

    public void Refresh()
    {
        animator.SetTrigger("Refresh");
    }
}
