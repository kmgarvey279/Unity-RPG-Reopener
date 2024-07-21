using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Tilemaps;

public class Battlefield : MonoBehaviour
{
    public CameraManager cameraManager;
    public GridManager gridManager;

    [Header("Environment")]
    [SerializeField] private GameObject environmentContainer;
    [SerializeField] private GameObject filter;


    private void Awake()
    {
        cameraManager = GetComponentInChildren<CameraManager>();
        gridManager = GetComponentInChildren<GridManager>();
    }

    public void LoadEnvironment(GameObject prefab)
    {
        GameObject environmentObject = Instantiate(prefab, environmentContainer.transform.position, Quaternion.identity);
        environmentObject.transform.parent = environmentContainer.transform;
    }
}
