using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Action : MonoBehaviour
{
    public Character character; 
    [Header("Action Properties")]
    public string animatorTrigger;
    public float range;
    [Header("Raycast")]
    //used for melee/projectile abilities that can be interupted by obsticles.
    public bool useRayCast;
    public LayerMask ignoreLayer;

    public void Start()
    {
        character = GetComponentInParent<Character>();
    }

    public virtual bool CheckConditions()
    {
        //check if target is in range && if nothing is in the way
        float distanceFromTarget = Vector3.Distance(character.targeter.currentTarget.transform.position, transform.position);
        if(distanceFromTarget <= range)
        {
            if(useRayCast)
            {
                RaycastHit2D hit = Physics2D.Raycast(transform.position, character.lookDirection, range, ~ignoreLayer);
                Debug.DrawRay(transform.position, character.lookDirection * range, Color.red, 3);
                if(hit.collider != null && hit.collider.gameObject.CompareTag(character.targeter.targetTag))
                {
                    return true;
                }
            }
            else
            {
                return true;
            }
        }
        return false;
    } 

    public virtual void TakeAction()
    {
        character.animator.SetTrigger(animatorTrigger);
    }
}

