using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class NPCTargeter : Targeter
{
    public GameObject lastAttacker;

    public override void AddTarget(GameObject obj)
    {       
        if(currentTarget == null)
        {
            currentTarget = obj;
        }
        base.AddTarget(obj);
    }

    public float GetProximityAggro(GameObject target)
    {
        float proxAggro;
        float distance = Vector3.Distance(transform.position, target.transform.position);
        if(distance < 1f)
        {
            proxAggro = 1.5f;
        }
        else if(distance < 2f)
        {
            proxAggro = 1.4f;
        }
        else if(distance < 3f)
        {
            proxAggro = 1.3f;
        }
        else if(distance < 4f)
        {
            proxAggro = 1.2f;
        }
        else if(distance < 3f)
        {
            proxAggro = 1.1f;
        }
        else 
        {
            proxAggro = 1f;
        }
        return proxAggro;
    }

    public override void SetNewTarget()
    {
        SetTargetWithHighestAggro();
    }

    public void UpdateDamageAggro(GameObject attacker, float damageAmount)
    {
        if(attacker != currentTarget)
        {
            SetTargetWithHighestAggro();
        }
        lastAttacker = attacker; 
        targets[attacker].damageAggro += damageAmount;
        StartCoroutine(ClearDamageAggro(attacker, damageAmount));
    }

    public IEnumerator ClearDamageAggro(GameObject attacker, float damageAmount)
    {
        yield return new WaitForSeconds(10f);
        if(targets[attacker] != null)
            targets[attacker].damageAggro -= damageAmount;
    }  

    public void SetTargetWithHighestAggro()
    {
        //default to closest target
        GameObject targetWithMostAggro = GetClosestTarget(); 
        float highestAggroValue = 0;

        foreach(KeyValuePair<GameObject, Aggro> target in targets)
        {        
            float thisAggroValue = target.Value.GetCurrentAggro() * GetProximityAggro(target.Key);
            if(target.Key == lastAttacker)
                thisAggroValue = thisAggroValue * 1.5f;
            if(thisAggroValue > highestAggroValue)
            {
                targetWithMostAggro = target.Key;
                highestAggroValue = thisAggroValue;
            }
        }
        currentTarget = targetWithMostAggro;
    }
}
