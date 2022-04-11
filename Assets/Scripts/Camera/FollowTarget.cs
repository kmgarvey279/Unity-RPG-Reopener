using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Cinemachine;

public class FollowTarget : MonoBehaviour
{
    [SerializeField] private string tagToFollow = "";
    private Transform target;
    private CinemachineVirtualCamera camera;

    private void Start()
    {
        camera = GetComponent<CinemachineVirtualCamera>();
        GameObject targetObject = GameObject.FindWithTag(tagToFollow);
        if(target != null)
        {
            camera.Follow = target.transform;
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
                    camera.Follow = target;
                }
            }
        }
    }
}
