using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SpawnEffect: Action
{
    [SerializeField] private GameObject effectPrefab;

    public override bool CheckConditions(GameObject target)
    {
        bool canTakeAction = true;
        foreach (Condition condition in conditions)
        {
            if(!condition.CheckCondition(character.gameObject, target, range))
                canTakeAction = false;
        }
        return canTakeAction;
    } 

    public override void TakeAction(GameObject target)
    {
        base.TakeAction(target);
        GameObject myObject = Instantiate(effectPrefab, target.transform.position, Quaternion.identity);
        DamageEffect effect = myObject.GetComponent<DamageEffect>();
        effect.SetAttackPower(character.characterInfo);
        effect.GetComponent<DamageEffect>().TriggerEffect();

    }
}
