using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Aggro
{
    public float damageAggro = 0;

    public float GetCurrentAggro()
    {
        return damageAggro;
    }
}

public class Targeter : MonoBehaviour
{
    public Dictionary<GameObject, Aggro> targets = new Dictionary<GameObject, Aggro>();
    public string targetTag;
    public GameObject currentTarget;

    public void RemoveTarget(GameObject target)
    {
        targets.Remove(target); 
        if(currentTarget == target)
        {
            SetNewTarget();
        }
    }

    public virtual void AddTarget(GameObject obj)
    {
        Aggro aggro = new Aggro();
        targets.Add(obj, aggro);
    }

    public void ChangeCurrentTarget(GameObject newTarget)
    {
        currentTarget = newTarget;
    }

    public virtual void SetNewTarget()
    {
    }

    public GameObject GetClosestTarget()
    {
        GameObject closestTarget = null;
        float smallestDistance = Mathf.Infinity;
        
        foreach(KeyValuePair<GameObject, Aggro> target in targets)
        {
            float thisDistance = Vector3.Distance(transform.position, target.Key.transform.position);

            if(thisDistance < smallestDistance)
            {
                smallestDistance = thisDistance;
                closestTarget = target.Key;
            }
        }
        return closestTarget;
    }
}
