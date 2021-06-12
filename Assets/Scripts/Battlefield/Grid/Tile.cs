using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.EventSystems;

public class Tile : MonoBehaviour, ISelectHandler, IDeselectHandler, IPointerEnterHandler
{
    public int x;
    public int y;
    public GameObject occupier;
    [SerializeField] private Image tileImage;
    [SerializeField] private Button tileButton;
    [SerializeField] private Image aoeImage;
    [SerializeField] private Image selectedImage;
    // [SerializeField] private Button button;
    // [SerializeField] private Color emptyColor;
    // [SerializeField] private Color occupiedColor;

    [SerializeField] private SignalSenderGO onTileSelect;

    private void Start()
    {
        float AlphaThreshold = 0.1f;
        tileImage.alphaHitTestMinimumThreshold = AlphaThreshold;
    }

    public void Display()
    {
        tileImage.enabled = true;
        tileButton.enabled = true;
    }

    public void ToggleAOE(bool isDisplayed)
    {
        aoeImage.enabled = isDisplayed;
    }

    public void Hide()
    {
        tileImage.enabled = false;
        tileButton.enabled = false;
        aoeImage.enabled = false;
        selectedImage.enabled = false;
    }

    public void Select()
    {
        EventSystem.current.SetSelectedGameObject(null);
        EventSystem.current.SetSelectedGameObject(this.gameObject);
    }

    public void OnPointerEnter(PointerEventData eventData)
    {
        Select();
    }

    // public void OnPointerExit(PointerEventData eventData)
    // {
    //     EventSystem.current.SetSelectedGameObject(null);
    // }

    public void OnSelect(BaseEventData eventData)
    {
        selectedImage.enabled = true;
    }

    public void OnDeselect(BaseEventData data)
    {
        selectedImage.enabled = false;
    }

    private void OnTriggerEnter2D(Collider2D other)
    {
        occupier = other.gameObject;
        if(occupier.CompareTag("Combatant") && other.isTrigger)
        {
            occupier.GetComponent<Combatant>().SetTile(this); 
        }
    }

    private void OnTriggerExit2D(Collider2D other)
    {
        if(other.gameObject.CompareTag("Combatant") && other.isTrigger)
        {
            occupier.GetComponent<Combatant>().SetTile(null); 
        }
        occupier = null;
    }

    public void OnClick()
    {
        onTileSelect.Raise(this.gameObject);
    }
}
