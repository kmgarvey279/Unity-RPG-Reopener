using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class PartyTab : MonoBehaviour
{
    [field: SerializeField] public PlayableCharacterID PlayableCharacterID { get; private set; }
    [SerializeField] private Image characterIcon;
    [SerializeField] private GameObject outline;
    [SerializeField] private GameObject overlay;

    private void Awake()
    {
        outline.SetActive(false);
        overlay.SetActive(false);
    }

    public void SetValues(PlayableCharacterID playableCharacterID, Sprite sprite)
    {
        PlayableCharacterID = playableCharacterID;
        characterIcon.sprite = sprite;
    }

    public void ToggleOverlay(bool setActive)
    {
        overlay.gameObject.SetActive(setActive);
    }

    public void ToggleSelected(bool setActive)
    {
        outline.gameObject.SetActive(setActive);
    }
}
