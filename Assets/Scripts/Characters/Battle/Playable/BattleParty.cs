using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System.Linq;
using StateMachineNamespace;

public class BattleParty : MonoBehaviour
{
    [Header("Party Scriptable Object")]
    public Party partyData;
    
    [Header("Party Member Prefabs")]
    [SerializeField] private GameObject claire;
    [SerializeField] private GameObject mutiny;
    [SerializeField] private GameObject shad;
    [SerializeField] private GameObject blaine;
    [SerializeField] private GameObject lucy;
    private Dictionary<string, GameObject> allyPrefabs = new Dictionary<string, GameObject>();
    
    [Header("Active Party Slots")]
    public List<Combatant> combatants = new List<Combatant>();

    [Header("Battle Positions")]
    public List<Transform> battlePositions = new List<Transform>();

    private void Awake()
    {
        allyPrefabs.Add("Claire", claire);
        allyPrefabs.Add("Mutiny", mutiny);
        allyPrefabs.Add("Shad", shad);  
        allyPrefabs.Add("Blaine", blaine);
        allyPrefabs.Add("Lucy", lucy);
        SpawnAllies();
    }

    public void SpawnAllies()
    {
        BattleManager battleManager = GetComponentInParent<BattleManager>();
        int allyCount = 0;
        for(int i = 0; i < partyData.partyList.Count; i++)
        {
            PartyMember partyMember = partyData.partyList[i];
            if(partyMember.inActiveParty)
            {
                string characterName = partyMember.characterName; 
                GameObject allyObject = Instantiate(allyPrefabs[characterName], battlePositions[allyCount].position, Quaternion.identity);
                allyObject.transform.parent = gameObject.transform;
                battleManager.AddPlayableCombatant(allyObject.GetComponent<Combatant>());
                allyCount++;
                if(allyCount == 3)
                    return;
            }
        }
    }
}