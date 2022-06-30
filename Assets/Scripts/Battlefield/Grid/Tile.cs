using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.EventSystems;
using TMPro;

public enum PathType
{
    Start,
    Straight, 
    Turn,
    End
}

public class Tile : MonoBehaviour, ISelectHandler, IDeselectHandler, IPointerEnterHandler
{
    public int x;
    public int y;
    public List<Combatant> occupiers = new List<Combatant>();
    [Header("Positions")]
    public Transform position1A;
    public Transform position2A;
    public Transform position2B;
    public Transform position3A;
    public Transform position3B;
    public Transform position3C;
    public Dictionary<int, List<Transform>> positionDict = new Dictionary<int, List<Transform>>();
    [Header("Display")]
    [SerializeField] private Image tileImage;
    [SerializeField] private Button tileButton;
    [SerializeField] private Image aoeImage;
    [SerializeField] private Image selectedImage;
    [Header("Path")]
    [SerializeField] private SpriteRenderer startPathImage;
    [SerializeField] private SpriteRenderer endPathImage;
    [SerializeField] private SpriteRenderer straightPathImage;
    [SerializeField] private SpriteRenderer turnPathImage;
    public GameObject[,] tileArray;
    [Header("Events")]
    [SerializeField] private SignalSenderGO onTileSelect;
    [Header("Color/Cost")]
    [SerializeField] private Color invisibleColor;
    [SerializeField] private Color selectableColor;
    [SerializeField] private Color unselectableColor;
    public int moveCost;

    private void OnEnable() 
    {
        positionDict.Add(1, new List<Transform>(){position1A});
        positionDict.Add(2, new List<Transform>(){position2A, position2B});
        positionDict.Add(3, new List<Transform>(){position3A, position3B, position3C});    
    }

    public void Display(bool targetable, bool enableButton)
    {
        if(targetable)
        {
            tileImage.color = selectableColor;
            if(enableButton)
            {
                tileButton.enabled = true;
            }
        }
        else
        {
            tileImage.color = unselectableColor;
        }
    }

    public void Hide()
    {
        tileImage.color = invisibleColor;
        tileButton.enabled = false;
        aoeImage.enabled = false;
        selectedImage.enabled = false;
    }

    public void DisplayAOE()
    {
        aoeImage.enabled = true;
    }

    public void HideAOE()
    {
        aoeImage.enabled = false;
    }

    public void DisplayPathNode(PathType pathType, Vector2 direction1, Vector2 direction2, CombatantType combatantType)
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
                if(combatantType == CombatantType.Enemy)
                {
                    z = 270;
                }
            }
            else if(direction1.y < 0)
            {
                z = 180;
            }
            else if(direction1.x > 0)
            {
                z = 270;
                if(combatantType == CombatantType.Enemy)
                {
                    z = 90;
                }
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
        if(straightPathImage.enabled)
            straightPathImage.enabled = false;
        if(turnPathImage.enabled)
            turnPathImage.enabled = false;
        if(endPathImage.enabled)
            endPathImage.enabled = false;
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

    public void AssignOccupier(Combatant occupier)
    {
        if(occupiers.Count == 3)
        {
            return;
        }
        occupiers.Add(occupier);
        MoveToPositions();
    }

    public void UnassignOccupier(Combatant occupier)
    {
        occupiers.Remove(occupier);
        MoveToPositions();
    }

    private void MoveToPositions()
    {
        for(int i = 0; i < occupiers.Count; i++)
        {
            occupiers[i].Move(positionDict[occupiers.Count][i], "Move");
        }
    }
}
