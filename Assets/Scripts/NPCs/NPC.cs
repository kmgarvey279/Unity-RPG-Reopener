using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class NPC : Character
{
    public override void Start()
    {
        base.Start();
        stunState = GetComponentInChildren<NPCStunState>(); 
        moveState = GetComponentInChildren<NPCMoveState>();
        dieState = GetComponentInChildren<NPCDieState>();
    }
}
