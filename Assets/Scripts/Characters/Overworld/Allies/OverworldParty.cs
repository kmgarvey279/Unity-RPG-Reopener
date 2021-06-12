using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System.Linq;
using StateMachineNamespace;

public class OverworldParty : MonoBehaviour
{
    [Header("Party Scriptable Object")]
    public Party partyData;
    
    [Header("Party Member Prefabs")]
    [SerializeField] private GameObject mutiny;
    [SerializeField] private GameObject shad;
    [SerializeField] private GameObject blaine;
    [SerializeField] private GameObject lucy;
    private Dictionary<string, GameObject> allyPrefabs = new Dictionary<string, GameObject>();
    
    [Header("Active Party Slots")]
    public List<GameObject> allyObjects = new List<GameObject>();

    private void Start()
    {
        allyPrefabs.Add("Mutiny", mutiny);
        allyPrefabs.Add("Shad", shad);  
        allyPrefabs.Add("Blaine", blaine);
        allyPrefabs.Add("Lucy", lucy);

        SpawnAllies();
    }

    public void SpawnAllies()
    {
        GameObject nextAlly = allyObjects[0];
        int partyOrder = 1;

        for(int i = 1; i < partyData.partyList.Count; i++)
        {
            PartyMember partyMember = partyData.partyList[i];
            if(partyMember.inActiveParty)
            {
                string name = partyMember.name; 
                Transform partyPosition = nextAlly.GetComponent<Ally>().followerPosition.transform;

                GameObject allyObject = Instantiate(allyPrefabs[name], partyPosition.position, Quaternion.identity);
                allyObject.transform.parent = gameObject.transform;
                allyObject.GetComponent<Follower>().partyPosition = partyPosition;
                allyObjects.Add(allyObject);

                nextAlly = allyObject;
                partyOrder++;
            }
        }
    }

    // public void UpdateCharacters()
    // {
    //     for(int i = 1; i < allyObjects.Count; i++)
    //     {
    //         GameObject allyToRemove = allyObjects[i];

    //         allyObjects.Remove(allyToRemove);
    //         Destroy(allyToRemove);
    //     }
    //     SpawnAllies();
    // }

    // public void AddTargetToAll(GameObject target)
    // {
    //     if(targets.Count <= 0)
    //     {
    //         StartBattle();
    //     }
    //     targets.Add(target);
    //     foreach (GameObject ally in activeParty)
    //     {   
    //         ally.GetComponent<Targeter>().AddTarget(target);   
    //     }
    // }

    // // public void RemoveTargetFromAll(GameObject target)
    // // {
    // //     targets.Remove(target);
    // //     foreach (GameObject ally in activeParty)
    // //     {   
    // //         ally.GetComponent<Targeter>().RemoveTarget(target);   
    // //     }
    // //     if(targets.Count <= 0)
    // //     {
    // //         EndBattle();
    // //     }
    // // }

    // public void AllySwap(int slotNum, string newAlly)
    // { 
    //     string oldAlly = partySlots[slotNum].name;
    //     Transform spawnLocation = partySlots[slotNum].transform;
    //     partyData.SwapPartyMembers(oldAlly, newAlly);
        
    //     Destroy(partySlots[slotNum].gameObject);

    //     GameObject allyObject = Instantiate(allyPrefabs[newAlly], spawnLocation.position, Quaternion.identity);
    //     PlayableCharacter character = allyObject.GetComponent<PlayableCharacter>();
    //     character.transform.parent = gameObject.transform;
    //     character.partyPosition = partySlots[slotNum - 1].followerPosition.transform;
    //     // partySlots[slotNum - 1].followerPosition.follower = allyObject;
    //     character.battlePosition = battlePositions[slotNum - 1];
            
    //     partySlots[slotNum] = character;
    // }

    // public void AllyAdd(int slotNum, string newAlly)
    // { 
    //     Transform partyPosition = partySlots[slotNum - 1].followerPosition.transform;

    //     GameObject allyObject = Instantiate(allyPrefabs[newAlly], partyPosition.position, Quaternion.identity);
    //     PlayableCharacter character = allyObject.GetComponent<PlayableCharacter>();
    //     character.transform.parent = gameObject.transform;
    //     character.partyPosition = partyPosition;
    //     // partySlots[slotNum - 1].followerPosition.follower = allyObject;
    //     character.battlePosition = battlePositions[slotNum - 1];
            
    //     partySlots[slotNum] = character;
    // }

    // public void AllyRemove(int slotNum)
    // { 
    //     Destroy(partySlots[slotNum].gameObject);

    //     partySlots[slotNum] = null;

    //     if(partySlots[slotNum].followerPosition.follower != null)
    //     {
    //         partySlots[slotNum].followerPosition.follower.partyPosition = partySlots[slotNum - 1].followerPosition.transform; 
    //     }
    // }

}