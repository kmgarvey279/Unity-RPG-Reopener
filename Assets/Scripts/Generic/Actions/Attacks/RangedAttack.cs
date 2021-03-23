using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class RangedAttack : Attack
{
    [SerializeField] private GameObject projectilePrefab;

    public override void TakeAction(Vector3 userDirection)
    {
        myAnimator.SetTrigger(animatorTrigger);
        GameObject projectileObject = Instantiate(projectilePrefab, transform.position, Quaternion.identity);
        Projectile projectile = projectileObject.GetComponent<Projectile>();
        projectile.Launch(userDirection, characterStats);
        Vector3 momentumVector = new Vector3(userDirection.x * attackMomentum, userDirection.y * attackMomentum);
        myRB.AddForce(momentumVector);
    }
}
