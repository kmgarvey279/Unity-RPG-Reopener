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

public enum PathType
{
    Start,
    Straight, 
    Turn,
    End
}

public class Tile : MonoBehaviour, ISelectHandler, IDeselectHandler, IPointerEnterHandler, IPointerExitHandler
{
    public int x;
    public int y;
    public Combatant occupier;
    public bool targetable;
    [Header("Display")]
    [SerializeField] private Image tileImage;
    [SerializeField] private Button tileButton;
    [SerializeField] private Image aoeImage;
    [SerializeField] private Image selectedImage;
    [SerializeField] private Image destinationImage;
    [Header("Path")]
    [SerializeField] private SpriteRenderer startPathImage;
    [SerializeField] private SpriteRenderer endPathImage;
    [SerializeField] private SpriteRenderer straightPathImage;
    [SerializeField] private SpriteRenderer turnPathImage;
    public GameObject[,] tileArray;
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
        targetable = true;
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

    public void DisplayPathNode(PathType pathType, Vector2 direction1, Vector2 direction2)
    {
        SpriteRenderer image;
            image = straightPathImage;
            if(pathType == PathType.Turn)
            {
                image = turnPathImage;
            }
            else if(pathType == PathType.Start)
            {
                image = startPathImage;
            }
            else if(pathType == PathType.End)
            {
                image = endPathImage;
            }
            int z = 0;
            if(direction1.y > 0)
            {
                z = 0;
            }
            else if(direction1.x < 0)
            {
                z = 90;
            }
            else if(direction1.y < 0)
            {
                z = 180;
            }
            else if(direction1.x > 0)
            {
                z = 270;
            }
            image.transform.rotation = Quaternion.Euler(0, 0, z);
            if(pathType == PathType.Turn)
            {
                if(direction1.x > 0 && direction2.y > 0 || direction1.x < 0 && direction2.y < 0 || direction1.y < 0 && direction2.x > 0 || direction1.y > 0 && direction2.x < 0)
                {
                    image.flipX = true;  
                } 
                else 
                {
                    image.flipX = false;
                    image.flipY = false;
                }
            }
        image.enabled = true;
    }

    public void HidePathNode()
    {
        if(startPathImage.enabled)
            startPathImage.enabled = false;
        if(endPathImage.enabled)
            endPathImage.enabled = false;
        if(straightPathImage.enabled)
            straightPathImage.enabled = false;
        if(turnPathImage.enabled)
            turnPathImage.enabled = false;
        // if(destinationImage.enabled)
        //     destinationImage.enabled = false;
    }

    // public void ChangeTargetability(Targetability newStatus)
    // {
    //     if(occupier)
    //     {
    //         targetability = newStatus;
    //         switch((int)targetability) 
    //         {
    //             //default
    //             case 0:
    //                 tileButton.enabled = false;
    //                 occupier.ClearSpriteMask();
    //                 break;
    //             //targetable
    //             case 1:
    //                 tileButton.enabled = true;
    //                 occupier.ClearSpriteMask();
    //                 break;
    //             //untargetable
    //             case 2:
    //                 tileButton.enabled = false;
    //                 occupier.GrayOut();
    //                 break;
    //         }
    //     }
    // }

    public void Hide()
    {
        tileImage.color = invisibleColor;
        tileButton.enabled = false;
        aoeImage.enabled = false;
        selectedImage.enabled = false;
        targetable = false;
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
        if(aoeImage.enabled && occupier)
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
        if(other.gameObject.CompareTag("Combatant") && other.isTrigger)
        {
            occupier = null; 
        }
    }

    public void OnClick()
    {
        onTileConfirm.Raise();
    }
}
