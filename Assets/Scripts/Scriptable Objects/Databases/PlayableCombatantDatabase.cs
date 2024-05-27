using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu(fileName = "NewPlayableCombatantDatabase", menuName = "Database/Playable Combatants")]
public class PlayableCombatantDatabase : ScriptableObject
{
    [SerializeField] private List<PlayableCombatantStaticInfo> PlayableCombatantInfo = new List<PlayableCombatantStaticInfo>();
    public Dictionary<PlayableCharacterID, PlayableCombatantStaticInfo> LookupTable { get; private set; }

    public void OnEnable()
    {
        LookupTable = new Dictionary<PlayableCharacterID, PlayableCombatantStaticInfo>();
        foreach (PlayableCombatantStaticInfo playableCombatantInfo in PlayableCombatantInfo)
        {
            if (!LookupTable.ContainsKey(playableCombatantInfo.PlayableCharacterID))
            {
                LookupTable.Add(playableCombatantInfo.PlayableCharacterID, playableCombatantInfo);
            }
        }
    }
}
