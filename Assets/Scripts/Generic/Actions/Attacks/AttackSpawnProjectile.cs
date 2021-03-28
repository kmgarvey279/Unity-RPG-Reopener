using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class AttackSpawnProjectile : Attack
{
    [SerializeField] private GameObject projectilePrefab;

    public override void TakeAction()
    {
        base.TakeAction();
        GameObject projectileObject = Instantiate(projectilePrefab, transform.position, Quaternion.identity);
        Projectile projectile = projectileObject.GetComponent<Projectile>();
        projectile.SetDamage(character.characterInfo);
        projectile.Launch(character.lookDirection);
    }
}
