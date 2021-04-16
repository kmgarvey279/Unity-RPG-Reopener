using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using StateMachineNamespace;

public class Character : MonoBehaviour
{
    public CharacterInfo characterInfo;
    [Header("Direction & Target")]
    public Vector3 lookDirection;
    [Header("Associated Prefabs")]
    public Afterimage afterimage; 
    
    public virtual void Start()
    {
        lookDirection = new Vector3(0,-1,0); 
    }

    public virtual void ChangeLookDirection(Vector3 newDirection)
    {
        lookDirection = newDirection;
    }

    // public virtual void OnDrawGizmos()
    // {
    //     if(targeter.currentTarget != null)
    //         Gizmos.DrawLine(transform.position, targeter.currentTarget.transform.position);
    // }
}
