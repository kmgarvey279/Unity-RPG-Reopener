using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.EventSystems;

public class Tile : MonoBehaviour, ISelectHandler, IDeselectHandler, IPointerEnterHandler, IPointerExitHandler
{
    public int x;
    public int y;
    public GameObject occupier;
    [Header("Display")]
    [SerializeField] private Image tileImage;
    [SerializeField] private Button tileButton;
    [SerializeField] private Image aoeImage;
    [SerializeField] private Image selectedImage;
    [Header("Events")]
    [SerializeField] private SignalSenderGO onTileSelect;
    [SerializeField] private SignalSender onTileConfirm;
    [Header("Color/Cost")]
    [SerializeField] private Color defaultColor;
    [SerializeField] private List<Color> costColors = new List<Color>();
    public int moveCost;

    public void Display(int moveCost = -1)
    {
        if(moveCost > -1)
        {
            this.moveCost = moveCost;
            tileImage.color = costColors[moveCost];
        }
        else
        {
            tileImage.color = defaultColor;
        }
        tileImage.enabled = true;
        tileButton.enabled = true;
    }

    public void ToggleAOE(bool isDisplayed)
    {
        aoeImage.enabled = isDisplayed;
    }

    public void Hide()
    {
        moveCost = -1;
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

    public void OnSelect(BaseEventData eventData)
    {
        onTileSelect.Raise(this.gameObject);
        selectedImage.enabled = true;
    }

    public void OnPointerEnter(PointerEventData eventData)
    {
        if(tileButton.enabled)
            Select();
    }

    public void OnDeselect(BaseEventData eventData)
    {
        selectedImage.enabled = false;
    }

    public void OnPointerExit(PointerEventData eventData)
    {
        // if(tileButton.enabled)
        //     EventSystem.current.SetSelectedGameObject(null);
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
        // if(other.gameObject.CompareTag("Combatant") && other.isTrigger)
        // {
        //     occupier.GetComponent<Combatant>().SetTile(null); 
        // }
        occupier = null;
    }

    public void OnClick()
    {
        onTileConfirm.Raise();
    }
}
