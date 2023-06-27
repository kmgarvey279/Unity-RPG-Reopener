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

    private void OnEnable()
    {
        animator = GetComponent<Animator>();
        spriteRenderer = GetComponent<SpriteRenderer>();
    }

    public void TriggerAnimation()
    {
        StartCoroutine(StartAnimationCo());
    }

    private IEnumerator StartAnimationCo()
    {
        yield return new WaitForSeconds(delay);
        //start animation
        animator.SetTrigger("Start");
        //start countdown to destruction
        if (!loop)
        {
            StartCoroutine(EndAnimationCo());
        }
    }

    private IEnumerator EndAnimationCo()
    {
        yield return new WaitForSeconds(duration);
        this.gameObject.SetActive(false);
    }

    public void ChangeSpriteMaterial(Material newMaterial)
    {
        spriteRenderer.material = newMaterial;
    }

    public void ChangeAnimatorSpeed(float newSpeed)
    {
        animator.speed = newSpeed;
    }
}
