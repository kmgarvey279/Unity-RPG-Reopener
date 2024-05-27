using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class VFXParent : MonoBehaviour
{
    [SerializeField] private List<VFXChild> vfxChildren = new List<VFXChild>();
    [SerializeField] private List<VFXMask> vfxMasks = new List<VFXMask>();
    [SerializeField] private float delay = 0;
    [SerializeField] private float duration = 0.6f;
    [SerializeField] private float moveDuration = 0f;
    [SerializeField] private bool loop = false;
    [Header("Rotation")]
    [SerializeField] private bool canFlip;

    private void Start()
    {
        moveDuration = Mathf.Clamp(moveDuration, 0, duration);
    }

    public void TriggerAnimation(bool shouldFlip = false)
    {
        StartCoroutine(StartAnimationCo(shouldFlip));
    }

    private IEnumerator StartAnimationCo(bool shouldFlip) 
    { 
        yield return new WaitForSeconds(delay);

        //mirror if action used by enemy
        if (canFlip && shouldFlip)
        {
            transform.rotation = Quaternion.Euler(transform.rotation.x, 180, transform.rotation.z);
        }

        //start animations
        foreach (VFXChild vfxChild in vfxChildren)
        {
            StartCoroutine(vfxChild.TriggerAnimationCo());
        }
        foreach (VFXMask vfxMask in vfxMasks)
        {
            StartCoroutine(vfxMask.TriggerAnimationCo());
        }

        //start countdown to destruction
        if (!loop)
        {
            StartCoroutine(EndAnimationCo());
        }
    }




    public IEnumerator MoveCo(Vector3 destination)
    {
        yield return new WaitForSeconds(delay);
        Vector2 start = transform.position;

        float timer = 0;
        while (this && transform.position != destination)
        {
            transform.position = Vector3.Lerp(start, destination, timer / moveDuration);
            timer += Time.deltaTime;

            yield return null;
        }

        if (this)
        {
            transform.position = destination;
        }
        yield return null;
    }



    private IEnumerator EndAnimationCo()
    {
        yield return new WaitForSeconds(duration);
        DestroySelf();
    }

    public void DestroySelf()
    {
        Destroy(this.gameObject);
    }


    public void StartTimeStop()
    {
        foreach (VFXChild vfxChild in vfxChildren)
        {
            vfxChild.OnInterventionStart();
        }
        foreach (VFXMask vfxMask in vfxMasks)
        {
            vfxMask.OnInterventionStart();
        }
    }

    public void EndTimeStop()
    {
        foreach (VFXChild vfxChild in vfxChildren)
        {
            vfxChild.OnInterventionEnd();
        }
        foreach (VFXMask vfxMask in vfxMasks)
        {
            vfxMask.OnInterventionEnd();
        }
    }
}
