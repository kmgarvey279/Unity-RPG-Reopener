using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SpawnProjectile : Action
{
    // [SerializeField] private GameObject projectilePrefab;

    // public override bool CheckConditions(GameObject target)
    // {
    //     bool canTakeAction = true;
    //     foreach (Condition condition in conditions)
    //     {
    //         if(!condition.CheckCondition(character.gameObject, target, range))
    //             canTakeAction = false;
    //     }
    //     return canTakeAction;
    // } 

    // public override void TakeAction(GameObject target)
    // {
    //     base.TakeAction(target);
    //     GameObject projectileObject = Instantiate(projectilePrefab, character.transform.position, Quaternion.identity);
    //     Projectile projectile = projectileObject.GetComponent<Projectile>();
    //     projectile.SetAttackPower(character.characterInfo);
    //     projectile.Launch(character.lookDirection);
    // }
}
