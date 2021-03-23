using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Action : MonoBehaviour
{
    [HideInInspector] public Animator myAnimator;
    [HideInInspector] public Rigidbody2D myRB;
    public LayerMask ignoreLayer;
    public string animatorTrigger;
    public CharacterStats characterStats;

    public void Start()
    {
        myAnimator = GetComponentInParent<Animator>();
        myRB = GetComponentInParent<Rigidbody2D>();
        characterStats = GetComponentInParent<CharacterStats>();
    }

    public virtual bool CheckConditions(Vector3 userLocation, Vector3 userDirection, string targetTag)
    {
        return false;
    } 

    public virtual void TakeAction(Vector3 userDirection)
    {
        myAnimator.SetTrigger(animatorTrigger);
    }
}
