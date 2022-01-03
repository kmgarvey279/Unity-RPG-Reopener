using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayableCharacterSpawner : MonoBehaviour
{
    [Header("Party Member Prefabs")]
    [SerializeField] private GameObject claire;
    [SerializeField] private GameObject mutiny;
    [SerializeField] private GameObject shad;
    [SerializeField] private GameObject blaine;
    [SerializeField] private GameObject lucy;
    private Dictionary<PlayableCharacterID, GameObject> playableCharacterPrefabs = new Dictionary<PlayableCharacterID, GameObject>();

    [Header("Spawn Positions")]
    public List<Transform> spawnPositions = new List<Transform>();

    private void Awake()
    {
        playableCharacterPrefabs.Add(PlayableCharacterID.Claire, claire);
        playableCharacterPrefabs.Add(PlayableCharacterID.Mutiny, mutiny);
        playableCharacterPrefabs.Add(PlayableCharacterID.Shad, shad);  
        playableCharacterPrefabs.Add(PlayableCharacterID.Blaine, blaine);
        playableCharacterPrefabs.Add(PlayableCharacterID.Lucy, lucy);
    }

    public Combatant SpawnPlayableCharacter(PlayableCharacterID playableCharacterID, int positionNum)
    {
        GameObject playableCharacterObject = Instantiate(playableCharacterPrefabs[playableCharacterID], spawnPositions[positionNum - 1].position, Quaternion.identity);
        playableCharacterObject.transform.parent = gameObject.transform;
        return playableCharacterObject.GetComponent<Combatant>();
    }
}