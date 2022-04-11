using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Tilemaps;

[System.Serializable]
public enum JumpType
{
    VerticalDown,
    VerticalUp,
    Horizontal
}

public class JumpTile : Interactable
{
    [SerializeField] private JumpType jumpType; 

    // public override void Interact()
    // {
    //     Vector3 destination = leader.transform.position;
    //     switch ((int)jumpType)
    //     {
    //         case 0:
    //             destination = new Vector3(transform.position.x, transform.position.y - 0.25f);
    //             break;
    //         case 1:
    //             destination = new Vector3(transform.position.x, transform.position.y + 0.25f);
    //             break;
    //         case 2:
    //             destination = new Vector3(transform.position.x, transform.position.y);
    //             break;
    //         default:
    //             break;
    //     }
    //     leader.Jump(jumpType, destination);
    // }
}
