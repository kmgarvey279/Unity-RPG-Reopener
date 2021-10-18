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
    public Combatant occupier;
    public Targetability targetability = Targetability.Default;
    [Header("Display")]
    [SerializeField] private Image tileImage;
    [SerializeField] private Button tileButton;
    [SerializeField] private Image aoeImage;
    [SerializeField] private Image selectedImage;
    [SerializeField] private Image pathNodeImage;
    [Header("Events")]
    [SerializeField] private SignalSenderGO onTileSelect;
    [SerializeField] private SignalSender onTileConfirm;
    [Header("Color/Cost")]
    [SerializeField] private Color inaccessibleColor;
    [SerializeField] private Color invisibleColor;
    [SerializeField] private Color defaultColor;
    public int moveCost;

    public void Display()
    {
        tileImage.color = defaultColor;
        tileButton.enabled = true;
    }

    public void DisplayAOE(bool targetPlayer, bool targetEnemy)
    {
        aoeImage.enabled = true;
        if(occupier)
        {
            if(occupier is PlayableCombatant && targetPlayer)
            {
                occupier.Select();
            }
            if(occupier is EnemyCombatant && targetEnemy)
            {
                occupier.Select();
            }
        }
    }

    public void HideAOE()
    {
        aoeImage.enabled = false;
        if(occupier)
        {
            occupier.Deselect();
        }
    }

    public void DisplayPathNode()
    {
        pathNodeImage.enabled = true;
    }

    public void HidePathNode()
    {
        pathNodeImage.enabled = false;
    }

    public void ChangeTargetability(Targetability newStatus)
    {
        if(occupier)
        {
            targetability = newStatus;
            switch((int)targetability) 
            {
                //default
                case 0:
                    tileButton.enabled = false;
                    occupier.ClearSpriteMask();
                    break;
                //targetable
                case 1:
                    tileButton.enabled = true;
                    occupier.ClearSpriteMask();
                    break;
                //untargetable
                case 2:
                    tileButton.enabled = false;
                    occupier.GrayOut();
                    break;
            }
        }
    }

    public void Hide()
    {
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
            occupier.Select();
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
            occupier.Deselect();
        }
    }

    public void OnPointerExit(PointerEventData eventData)
    {
        // if(tileButton.enabled)
        //     EventSystem.current.SetSelectedGameObject(null);
    }

    private void OnTriggerEnter2D(Collider2D other)
    {
        if(other.gameObject.CompareTag("Combatant") && other.isTrigger)
        {
            Combatant combatant = other.GetComponent<Combatant>();
            combatant.SetTile(this); 
            occupier = combatant;
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
