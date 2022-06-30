using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayableCharacterSpawner : MonoBehaviour
{
    [SerializeField] private Camera mainCamera;
    [Header("Party Member Prefabs")]
    [SerializeField] private GameObject claire;
    [SerializeField] private GameObject mutiny;
    [SerializeField] private GameObject shad;
    [SerializeField] private GameObject blaine;
    [SerializeField] private GameObject lucy;
    [SerializeField] private GameObject oshi;
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
        playableCharacterPrefabs.Add(PlayableCharacterID.Oshi, oshi);
    }

    public Combatant SpawnPlayableCharacter(PlayableCharacterInfo playableCharacterInfo, int positionNum)
    {
        if(positionNum <= spawnPositions.Count)
        {
            Tile tile = spawnPositions[positionNum - 1];
            GameObject playableCharacterObject = Instantiate(playableCharacterPrefabs[playableCharacterInfo.playableCharacterID], tile.transform.position, Quaternion.identity);
            //set transform
            playableCharacterObject.transform.parent = gameObject.transform;
            Combatant combatant = playableCharacterObject.GetComponent<Combatant>();
            //set data
            combatant.SetCharacterData(playableCharacterInfo);
            //set tile
            tile.AssignOccupier(combatant);
            combatant.tile = tile;
            //set camera for ui display
            combatant.targetSelect.canvas.worldCamera = mainCamera;
            
            return combatant;
        }
        return null;
    }
}