using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class Connector : MonoBehaviour
{
    public Transform spawnPoint;
    public Entrance entrance;
    public OverworldData overworldData;
    [Header("Events")]
    public SignalSender onScreenFadeIn;
    public SignalSender onScreenFadeOut;

    public void PlayerEnter(GameObject player)
    {
        overworldData.lockInput = true;
        onScreenFadeOut.Raise();
        StartCoroutine(ResolveEnterCo(player));
    }

    public virtual IEnumerator ResolveEnterCo(GameObject player)
    {
        yield return new WaitForSeconds(1f); 
    }


    private IEnumerator ExitSceneCo(GameObject player)
    {
        overworldData.lockInput = true;
        onScreenFadeOut.Raise();
        yield return new WaitForSeconds(1f); 

    }
}
