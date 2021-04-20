using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using StateMachineNamespace;
using Pathfinding;

public class Character : MonoBehaviour
{
    public CharacterInfo characterInfo;
    [Header("Direction")]
    public Vector3 lookDirection;
    [Header("Associated Prefabs")]
    public Afterimage afterimage; 
    [Header("GameObject Components")]
    public Rigidbody2D rigidbody;
    public Animator animator;
    public BoxCollider2D boxCollider;
    [Header("A* Pathfinding")]
    public AIPath aiPath;
    public AIDestinationSetter setter;
    
    public virtual void Start()
    {
        lookDirection = new Vector3(0,-1,0); 
        rigidbody = GetComponent<Rigidbody2D>();
        animator = GetComponent<Animator>();
        boxCollider = GetComponent<BoxCollider2D>();
        aiPath = GetComponent<AIPath>();
        setter = GetComponent<AIDestinationSetter>();
    }

    public virtual void ChangeLookDirection(Vector3 newDirection)
    {
        lookDirection = newDirection;
    }
}
