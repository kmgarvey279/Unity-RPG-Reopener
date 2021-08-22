using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.EventSystems;

public enum Targetability
{
    Default,
    Targetable,
    Untargetable
}

public class Tile : MonoBehaviour, ISelectHandler, IDeselectHandler, IPointerEnterHandler, IPointerExitHandler
{
    public int x;
    public int y;
    public GameObject occupier;
    private Targetability targetability = Targetability.Default;
    [Header("Display")]
    [SerializeField] private Image tileImage;
    [SerializeField] private Button tileButton;
    [SerializeField] private Image aoeImage;
    [SerializeField] private Image selectedImage;
    [Header("Events")]
    [SerializeField] private SignalSenderGO onTileSelect;
    [SerializeField] private SignalSender onTileConfirm;
    [Header("Color/Cost")]
    [SerializeField] private Color invisibleColor;
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
        tileButton.enabled = true;
    }

    public void DisplayAOE(bool targetFriendly, bool targetHostile)
    {
        aoeImage.enabled = true;
        if(occupier)
        {
            Combatant combatant = occupier.GetComponent<Combatant>();
            if(combatant)
            {
                if(combatant is AllyCombatant && targetFriendly)
                {
                    combatant.Select();
                }
                if(combatant is EnemyCombatant && targetHostile)
                {
                    combatant.Select();
                }
            }
        }
    }

    public void ClearAOE()
    {
        aoeImage.enabled = false;
        if(occupier)
        {
            Combatant combatant = occupier.GetComponent<Combatant>();
            if(combatant)
                combatant.Deselect();
        }
    }

    public void ChangeTargetability(Targetability newStatus)
    {
        if(occupier)
        {
            Combatant combatant = occupier.GetComponent<Combatant>();
            if(combatant)
            {
                targetability = newStatus;
                switch((int)targetability) 
                {
                    //default
                    case 0:
                        tileButton.enabled = false;
                        combatant.ClearSpriteMask();
                        break;
                    //targetable
                    case 1:
                        tileButton.enabled = true;
                        combatant.ClearSpriteMask();
                        break;
                    //untargetable
                    case 2:
                        tileButton.enabled = false;
                        combatant.GrayOut();
                        break;
                }
            }
        }
    }

    public void Hide()
    {
        moveCost = -1;
        tileImage.color = invisibleColor;
        tileButton.enabled = false;
        aoeImage.enabled = false;
        selectedImage.enabled = false;
        // ChangeTargetability(Targetability.Default);
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
        if(occupier && targetability == Targetability.Targetable)
        {
            Combatant combatant = occupier.GetComponent<Combatant>();
            if(combatant)
                combatant.Select();
        }
    }

    public void OnPointerEnter(PointerEventData eventData)
    {
        if(tileButton.enabled)
            Select();
    }

    public void OnDeselect(BaseEventData eventData)
    {
        selectedImage.enabled = false;
        if(occupier)
        {
            Combatant combatant = occupier.GetComponent<Combatant>();
            combatant.Deselect();
        }
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
