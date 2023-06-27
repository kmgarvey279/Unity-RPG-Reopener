using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Glow : MonoBehaviour
{
    [SerializeField] private Animator animator;
    public void ToggleGlow(bool isOn)
    {
        animator.SetBool("On", isOn);
    }
}
