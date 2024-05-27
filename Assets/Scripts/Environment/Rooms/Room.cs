using System.Collections;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;

public class Room : MonoBehaviour
{
    //private bool isActive = false;
    [SerializeField] private OverworldData overworldData;
    [SerializeField] private OverworldCameraManager cameraManager;

    [SerializeField] private List<EnemyContainer> enemies = new List<EnemyContainer>();
    [SerializeField] private GameObject enemyParent;

    [SerializeField] private List<NPC> npcs = new List<NPC>();
    [SerializeField] private GameObject npcParent;

    private void Awake()
    {
        enemies = enemyParent.GetComponentsInChildren<EnemyContainer>().ToList();
        npcs = npcParent.GetComponentsInChildren<NPC>().ToList();
        DeactivateRoom();
    }

    public void ActivateRoom(GameObject playerObject)
    {
        cameraManager.Activate(playerObject);

        for (int i = 0; i < enemies.Count; i++)
        {
            enemies[i].ActivateEnemy(true);
            //enemies[i].OnRoomActivate();
        }

        for (int i = 0; i < npcs.Count; i++)
        {
            npcs[i].gameObject.SetActive(true);
        }

        //isActive = true;
        overworldData.SetCurrentRoom(this);
    }

    public void DeactivateRoom()
    {
        cameraManager.Deactivate();

        for (int i = 0; i < enemies.Count; i++)
        {
            enemies[i].ActivateEnemy(false);
        }

        for (int i = 0; i < npcs.Count; i++)
        {
            npcs[i].gameObject.SetActive(false);
        }

        //isActive = false;
    }
}
