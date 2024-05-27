using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.EventSystems;

public class Tile : MonoBehaviour
{
    [field: SerializeField, Range(0,2)] public int X { get; private set; }
    [field: SerializeField, Range(0,2)] public int Y { get; private set; }
    public Combatant Occupier { get; private set; }

    public void AssignOccupier(Combatant occupier)
    {
        if (Occupier)
        {
            return;
        }
        Occupier = occupier;
        //Occupier.Move(transform, "Move");
    }

    public void UnassignOccupier()
    {
        Occupier = null;
    }
}
