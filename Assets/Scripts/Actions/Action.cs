using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Action : MonoBehaviour
{
    public Character character; 
    [Header("Action Properties")]
    public string animatorTrigger;
    public float range;
    public GameObject target;
    [Header("Conditions")]
    public Condition[] conditions;

    public virtual void Start()
    {
        character = GetComponentInParent<Character>();
    }

    public virtual bool CheckConditions(GameObject target)
    {
        return false;
    } 

    public virtual void TakeAction(GameObject target)
    {
        // character.animator.SetTrigger(animatorTrigger);
    }
}

