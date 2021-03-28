using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Attack : Action
{
    public float momentum;
    
    public override void TakeAction()
    {
        base.TakeAction();
        Vector3 momentumVector = new Vector3(character.lookDirection.x * momentum, character.lookDirection.y * momentum);
        character.rigidbody.AddForce(momentumVector);
    }
}
