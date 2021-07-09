using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Cinemachine;

public enum CameraType
{
    ZoomIn,
    ZoomOut
}

public class CameraManager : MonoBehaviour
{
    [Header("Cameras")]
    [SerializeField] private CinemachineVirtualCamera zoomInCam;
    [SerializeField] private CinemachineVirtualCamera zoomOutCam;
    private CinemachineVirtualCamera activeCam;

    private void Awake()
    {
        activeCam = zoomOutCam;
    }

    public void ZoomOut()
    {
        if(activeCam != zoomOutCam)
        {
            activeCam.gameObject.SetActive(false);
            zoomOutCam.gameObject.SetActive(true);
            activeCam = zoomOutCam; 
        }
    }

    public void ZoomIn(GameObject target)
    {
        zoomInCam.Follow = target.transform;

        if(activeCam != zoomInCam)
        {
            activeCam.gameObject.SetActive(false);
            zoomInCam.gameObject.SetActive(true);
            activeCam = zoomInCam; 
        }
    }
}
