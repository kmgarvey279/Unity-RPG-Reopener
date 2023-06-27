using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class RoomConnector : Connector
{
    public RoomConnector destination;

    public override IEnumerator ResolveEnterCo(GameObject player)
    {
        overworldData.lockInput = true;
        onScreenFadeOut.Raise();
        yield return new WaitForSeconds(1f); 

        player.transform.position = destination.spawnPoint.position;
        onScreenFadeIn.Raise();
        yield return new WaitForSeconds(1f);

        overworldData.lockInput = false;
    }
}
