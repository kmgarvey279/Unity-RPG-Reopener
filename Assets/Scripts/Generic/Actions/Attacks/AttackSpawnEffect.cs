using System.Collections;
using System.Collections.Generic;
using UnityEngine;

//multiple targets
public class AttackSpawnEffect : Attack
{
    [SerializeField] private GameObject effectPrefab;

    public override void TakeAction()
    {
        base.TakeAction();
        GameObject myObject = Instantiate(effectPrefab, transform.position, Quaternion.identity);
        Effect effect = myObject.GetComponent<Effect>();
        effect.SetDamage(character.characterInfo);
        effect.GetComponent<Effect>().TriggerEffect();

    }
}
