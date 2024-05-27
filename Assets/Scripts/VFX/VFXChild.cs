using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class VFXChild : MonoBehaviour
{
    private Animator animator;
    private SpriteRenderer spriteRenderer;
    [Header("Timing")]
    [SerializeField] private float delay = 0;
    [SerializeField] private float duration = 0;
    [SerializeField] private bool loop = false;
    private Color defaultColor = new Color();
    private Color greyscaleColor = new Color32(111, 111, 111, 100);

    private void OnEnable()
    {
        animator = GetComponent<Animator>();
        if (animator != null)
        {
            animator.enabled = false;
        }
        spriteRenderer = GetComponent<SpriteRenderer>();
        if (spriteRenderer != null)
        {
            spriteRenderer.enabled = false;
            defaultColor = spriteRenderer.material.color;
        }
    }

    public IEnumerator TriggerAnimationCo()
    {
        yield return new WaitForSeconds(delay);

        StartAnimation();
    }

    public void StartAnimation()
    {
        if (spriteRenderer != null)
        {
            spriteRenderer.enabled = true;
        }
        if (animator != null)
        {
            animator.enabled = true;
        }
        if (!loop)
        {
            StartCoroutine(EndAnimationCo());
        }
    }

    private IEnumerator EndAnimationCo()
    {
        yield return new WaitForSeconds(duration);

        StopAnimation();
    }

    public void StopAnimation()
    {
        if (animator != null)
        {
            animator.enabled = false;
        }
        if (spriteRenderer != null)
        {
            spriteRenderer.enabled = false;
        }
    }

    public void OnInterventionStart()
    {
        if (spriteRenderer != null)
            spriteRenderer.material.color = greyscaleColor;

        if (animator != null)
            animator.speed = 0;
    }

    public void OnInterventionEnd()
    {
        if (spriteRenderer != null)
            spriteRenderer.material.color = defaultColor;

        if (animator != null)
            animator.speed = 1;
    }
}
