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
    public List<Tile> spawnPositions = new List<Tile>();

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
        if(positionNum <= spawnPositions.Count)
        {
            Tile tile = spawnPositions[positionNum - 1];
            GameObject playableCharacterObject = Instantiate(playableCharacterPrefabs[playableCharacterID], tile.transform.position, Quaternion.identity);
            playableCharacterObject.transform.parent = gameObject.transform;

            Combatant combatant = playableCharacterObject.GetComponent<Combatant>();
            tile.AssignOccupier(combatant);
            combatant.tile = tile;
            return combatant;
        }
        return null;
    }
}