using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Tilemaps;

public class BattleEnvironment : MonoBehaviour
{
    [SerializeField] private List<TilemapRenderer> tilemapRenderers = new List<TilemapRenderer>();
    [SerializeField] private Material defaultMaterial;
    [SerializeField] private Material greyscaleMaterial;

    public void StartTimeStop()
    {
        foreach (TilemapRenderer tilemapRenderer in tilemapRenderers)
        {
            tilemapRenderer.material = greyscaleMaterial;
        }
    }

    public void EndTimeStop()
    {
        foreach (TilemapRenderer tilemapRenderer in tilemapRenderers)
        {
            tilemapRenderer.material = defaultMaterial;
        }
    }
}
