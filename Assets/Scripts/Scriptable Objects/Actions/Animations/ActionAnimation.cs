using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[System.Serializable]
[CreateAssetMenu(fileName = "New Action Animation", menuName = "Action/Animaton")]
public class ActionAnimation : ScriptableObject
{   
    // public SignalSender onTriggerActionEffect;

    // public void TriggerAnimation(Combatant combatant, Vector3 targetLocation, GameObject graphicPrefab)
    // {
        // if(animatorTrigger != null)
        // {
        //     combatant.animator.SetTrigger(animatorTrigger);
        // }

        // StartCoroutine(SpawnGraphicCo(targetLocation, graphicPrefab));
    // }

    // public IEnumerator SpawnGraphicCo(Vector3 spawnLocation, GameObject graphicPrefab)
    // {
        // yield return 0.5f;
        // if(graphicPrefab != null)
        // {
        //     Instantiate(graphicPrefab, spawnLocation, Quaternion.identity);
        // }
        // yield return 1f;
        // onTriggerActionEffect.Raise();
        // if(graphicObject.GetComponent<ActionGraphic>() is ProjectileGraphic)
        // {
        //     ProjectileGraphic projectileGraphic = graphicObject.GetComponent<ProjectileGraphic>();
        //     projectileGraphic.destination = destination;
        // }
    // }
}
