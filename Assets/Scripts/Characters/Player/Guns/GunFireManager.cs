using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class GunFireManager : MonoBehaviour
{
    public Gun gunStats;
    public Transform firePoint;
    [HideInInspector] public float gunRecoil;
    [HideInInspector] public float gunDelay;
    [HideInInspector] public Rigidbody2D playerRB;
    [HideInInspector] public Animator playerAnimator;
}
