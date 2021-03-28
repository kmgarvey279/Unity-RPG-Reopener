using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Effect : MonoBehaviour
{
    private Animator myAnimator;

    public void Start()
    {
        myAnimator = GetComponent<Animator>();    
    }

    public virtual void SetDamage(CharacterInfo characterInfo)
    {
    }

    public void TriggerEffect()
    {
        myAnimator.SetTrigger("Start");
    }

    public void DestroyEffect()
    {
        Destroy(this.gameObject);
    }
}
