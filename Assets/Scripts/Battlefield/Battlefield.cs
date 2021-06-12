using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Battlefield : MonoBehaviour
{
    public CameraManager cameraManager;
    public GridManager gridManager;

    private void Awake()
    {
        cameraManager = GetComponentInChildren<CameraManager>();
        gridManager = GetComponentInChildren<GridManager>();
    }
}
