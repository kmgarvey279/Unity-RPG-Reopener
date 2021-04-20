using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayableCharacter : Character
{
    public Transform partyPosition;
    public FollowerPosition followerPosition;
    public Transform battlePosition;

    public override void Start()
    {
        base.Start();
        // followerPosition = GetComponentInChildren<FollowerPosition>();  
    }
}
