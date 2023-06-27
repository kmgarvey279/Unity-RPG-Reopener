using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Cinemachine;

public class FollowTarget : MonoBehaviour
{
    [SerializeField] private string tagToFollow = "";
    private Transform target;
    private CinemachineVirtualCamera vCam;

    private void Start()
    {
        vCam = GetComponent<CinemachineVirtualCamera>();
        GameObject targetObject = GameObject.FindWithTag(tagToFollow);
        if(target != null)
        {
            vCam.Follow = target.transform;
        }
    }

    private void Update()
    {
        if(tagToFollow != "")
        {
            if(target == null)
            {
                target = GameObject.FindWithTag("Player").transform;
                if(target != null)
                {
                    vCam.Follow = target;
                }
            }
        }
    }
}
