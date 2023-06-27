using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Cinemachine;


public class CameraManager : MonoBehaviour
{
    [Header("Cameras")]
    [SerializeField] private CinemachineVirtualCamera followCam;
    [SerializeField] private CinemachineVirtualCamera defaultCam;

    public void Unfollow()
    {
        followCam.gameObject.SetActive(false);
        defaultCam.gameObject.SetActive(true);
    }

    public void Follow(GameObject target)
    {
        followCam.Follow = target.transform;

        defaultCam.gameObject.SetActive(false);
        followCam.gameObject.SetActive(true);
    }
}
