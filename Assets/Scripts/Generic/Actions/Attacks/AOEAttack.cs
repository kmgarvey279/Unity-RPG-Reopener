using System.Collections;
using System.Collections.Generic;
using UnityEngine;

//multiple targets
public class AOEAttack : Attack
{
    [SerializeField] private GameObject effectPrefab;
    private GameObject[] targets;

    public override bool CheckConditions(Vector3 userLocation, Vector3 userDirection, string targetTag)
    {
        targets = GameObject.FindGameObjectsWithTag(targetTag);
        if(targets.Length > 0)
        {
            foreach (GameObject target in targets)
            {
                if(Vector3.Distance(target.transform.position, userLocation) <= attackRange)
                {
                    return true;
                }
            }
        }
        return false;
    } 

    public override void TakeAction(Vector3 userDirection)
    {
        myAnimator.SetTrigger(animatorTrigger);
        GameObject myObject = Instantiate(effectPrefab, transform.position, Quaternion.identity);
        myObject.GetComponent<AoEHitboxManager>().StartAttack(characterStats);

    }
}
