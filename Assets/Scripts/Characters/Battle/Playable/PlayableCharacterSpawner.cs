using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayableCharacterSpawner : MonoBehaviour
{
    [Header("Dependencies")]
    [SerializeField] private Camera mainCamera;
    [SerializeField] private BattleManager battleManager;
    [Header("Party Member Prefabs")]
    [SerializeField] private GameObject claire;
    [SerializeField] private GameObject mutiny;
    [SerializeField] private GameObject shad;
    [SerializeField] private GameObject blaine;
    [SerializeField] private GameObject lucy;
    [SerializeField] private GameObject oshi;
    private Dictionary<PlayableCharacterID, GameObject> playableCharacterPrefabs = new Dictionary<PlayableCharacterID, GameObject>();

    private void Awake()
    {
        playableCharacterPrefabs.Add(PlayableCharacterID.Claire, claire);
        playableCharacterPrefabs.Add(PlayableCharacterID.Mutiny, mutiny);
        playableCharacterPrefabs.Add(PlayableCharacterID.Shad, shad);  
        playableCharacterPrefabs.Add(PlayableCharacterID.Blaine, blaine);
        playableCharacterPrefabs.Add(PlayableCharacterID.Lucy, lucy);
        playableCharacterPrefabs.Add(PlayableCharacterID.Oshi, oshi);
    }

    public PlayableCombatant SpawnPlayableCharacter(PlayableCharacterInfo playableCharacterInfo, Tile tile, PlayableCharacterID linkedCharacter)
    {
        GameObject playableCharacterObject = Instantiate(playableCharacterPrefabs[playableCharacterInfo.PlayableCharacterID], tile.transform.position, Quaternion.identity);
        //set transform
        playableCharacterObject.transform.parent = transform;
        PlayableCombatant playableCombatant = playableCharacterObject.GetComponent<PlayableCombatant>();
        if(playableCombatant)
        {
            //set data
            playableCombatant.SetCharacterData(playableCharacterInfo, linkedCharacter);
            //set tile
            tile.AssignOccupier(playableCombatant);
            playableCombatant.SetTile(tile);
            //set camera for ui display
            playableCombatant.SetBattleManager(battleManager);
            playableCombatant.SetUICamera(mainCamera);
            return playableCombatant;
        }
        return null;
    }
}