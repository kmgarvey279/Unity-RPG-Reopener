using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class SaveSlotPartyIcon : MonoBehaviour
{
    [SerializeField] private Image characterIcon;
    [SerializeField] private GameObject reserveOverlay;

    public void SetValues(Sprite sprite)
    {
        characterIcon.sprite = sprite;
    }
}
