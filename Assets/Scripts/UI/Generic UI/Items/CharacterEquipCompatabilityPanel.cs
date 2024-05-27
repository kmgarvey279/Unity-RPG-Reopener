using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class CharacterEquipCompatabilityPanel : MonoBehaviour
{
    [SerializeField] private GameObject content;
    [Header("Panels")]
    [SerializeField] private GameObject clairePanel;
    [SerializeField] private GameObject claireOverlay;
    [SerializeField] private GameObject mutinyPanel;
    [SerializeField] private GameObject mutinyOverlay;
    [SerializeField] private GameObject blainePanel;
    [SerializeField] private GameObject blaineOverlay;
    [SerializeField] private GameObject shadPanel;
    [SerializeField] private GameObject shadOverlay;
    [SerializeField] private GameObject oshiPanel;
    [SerializeField] private GameObject oshiOverlay;
    [SerializeField] private GameObject lucyPanel;
    [SerializeField] private GameObject lucyOverlay;
    private Dictionary<PlayableCharacterID, GameObject> panelDict;
    private Dictionary<PlayableCharacterID, GameObject> overlayDict;

    private void Awake()
    {
        if (content.activeInHierarchy)
        {
            content.SetActive(false);
        }

        panelDict = new Dictionary<PlayableCharacterID, GameObject>
        {
            { PlayableCharacterID.Claire, clairePanel },
            { PlayableCharacterID.Mutiny, mutinyPanel },
            { PlayableCharacterID.Blaine, blainePanel },
            { PlayableCharacterID.Shad, shadPanel },
            { PlayableCharacterID.Oshi, oshiPanel },
            { PlayableCharacterID.Lucy, lucyPanel }
        };

        overlayDict = new Dictionary<PlayableCharacterID, GameObject>
        {
            { PlayableCharacterID.Claire, claireOverlay },
            { PlayableCharacterID.Mutiny, mutinyOverlay },
            { PlayableCharacterID.Blaine, blaineOverlay },
            { PlayableCharacterID.Shad, shadOverlay },
            { PlayableCharacterID.Oshi, oshiOverlay },
            { PlayableCharacterID.Lucy, lucyOverlay }
        };
    }
 
    public void DisplayItemCompatability(EquipmentItem item)
    {
        foreach (KeyValuePair<PlayableCharacterID, GameObject> entry in panelDict)
        {
            if (SaveManager.Instance.LoadedData.PlayerData.PartyData.PartyMembers.Contains(entry.Key))
            {
                entry.Value.SetActive(true);
                if (!item.CharacterExclusive || item.ExclusiveCharacters.Contains(entry.Key))
                {
                    overlayDict[entry.Key].SetActive(false);
                }
                else
                {
                    overlayDict[entry.Key].SetActive(true);
                }
            }
            else
            {
                entry.Value.SetActive(false);
            }
        }
        if (!content.activeInHierarchy)
        {
            content.SetActive(true);
        }
    }

    public void Hide()
    {
        if (content.activeInHierarchy)
        {
            content.SetActive(false);
        }
    }
}
