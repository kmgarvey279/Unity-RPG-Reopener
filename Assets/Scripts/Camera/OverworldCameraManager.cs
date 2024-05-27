using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Cinemachine;

public class OverworldCameraManager : MonoBehaviour
{
    [SerializeField] private CinemachineVirtualCamera vCam;

    public void Activate(GameObject target)
    {
        vCam.Follow = target.transform;
        vCam.gameObject.SetActive(true);
    }

    public void Deactivate()
    {
        vCam.Follow = null;
        vCam.gameObject.SetActive(false);
    }
}
