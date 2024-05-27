using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Tilemaps;

public class Battlefield : MonoBehaviour
{
    public CameraManager cameraManager;
    public GridManager gridManager;
    [SerializeField] private List<TilemapRenderer> spriteRenderers = new List<TilemapRenderer>();
    [SerializeField] private GameObject filter;
    [SerializeField] private Material defaultMaterial;
    [SerializeField] private Material greyscaleMaterial;

    private void Awake()
    {
        cameraManager = GetComponentInChildren<CameraManager>();
        gridManager = GetComponentInChildren<GridManager>();
    }

    public void StartTimeStop()
    {
        foreach (TilemapRenderer tilemapRenderer in spriteRenderers)
        {
            tilemapRenderer.material = greyscaleMaterial;
        }
    }

    public void EndTimeStop()
    {
        foreach (TilemapRenderer tilemapRenderer in spriteRenderers)
        {
            tilemapRenderer.material = defaultMaterial;
        }
    }
}
