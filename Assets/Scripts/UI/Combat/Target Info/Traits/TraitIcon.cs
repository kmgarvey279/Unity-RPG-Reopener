using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;
using UnityEngine.EventSystems;
using UnityEngine.UI;

public class TraitIcon : MonoBehaviour, ISelectHandler, IDeselectHandler
{
    public Trait Trait { get; private set; }
    [SerializeField] private Image iconImage;
    private Button button;
    [SerializeField] private GameObject glowingSquare;
    [SerializeField] private SignalSenderGO onSelectTraitIcon;

    private void Awake()
    {
        button = GetComponent<Button>();
    }

    public void AssignTrait(Trait trait)
    {
        Trait = trait;
        iconImage.sprite = trait.Icon;
    }

    public void ToggleButton(bool isActive)
    {
        button.interactable = isActive;
    }

    public void OnSelect(BaseEventData eventData)
    {
        glowingSquare.SetActive(true);
        onSelectTraitIcon.Raise(this.gameObject);
    }

    public void OnDeselect(BaseEventData eventData)
    {
        glowingSquare.SetActive(false);
    }
}
