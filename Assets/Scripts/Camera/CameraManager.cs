using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Cinemachine;

public class CameraManager : MonoBehaviour
{
   [SerializeField] private CinemachineVirtualCamera followCamera;
//    [SerializeField] private CinemachineVirtualCamera followCameraMid;
   [SerializeField] private CinemachineVirtualCamera battlefieldCamera;
   public enum CameraType
   {
       Follow,
       Battlefield
   }
   public CameraType selectedCamera;

   private void Awake()
   {
       selectedCamera = CameraType.Battlefield;
   }

    public void SetTarget(Transform newTransform)
    {
        if(selectedCamera != CameraType.Follow)
        {
            battlefieldCamera.gameObject.SetActive(false);
            followCamera.gameObject.SetActive(true);
        }
        followCamera.Follow = newTransform;
    }

    public void ZoomOut()
    {
        followCamera.gameObject.SetActive(false);
        battlefieldCamera.gameObject.SetActive(true);
    }

}
