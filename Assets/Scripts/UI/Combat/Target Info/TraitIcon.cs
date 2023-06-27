using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;
using UnityEngine.UI;

public class TraitIcon : MonoBehaviour
{
    public TraitInstance TraitInstance { get; private set; }
    [SerializeField] private Image iconImage;
    [SerializeField] private GameObject glowingSquare;

    public void AssignTrait(TraitInstance traitInstance)
    {
        TraitInstance = traitInstance;
        iconImage.sprite = traitInstance.Trait.Icon;
    }

    public void OnSelect()
    {
        glowingSquare.SetActive(true);
    }

    public void OnDeselect()
    {
        glowingSquare.SetActive(false);
    }
}
