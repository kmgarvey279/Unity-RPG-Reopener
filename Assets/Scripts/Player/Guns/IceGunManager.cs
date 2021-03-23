using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class IceGunManager : GunFireManager
{
    [SerializeField] private GameObject iceBlast;

    private void Start()
    {
        playerRB = GetComponentInParent<Rigidbody2D>(); 
        playerAnimator = GetComponentInParent<Animator>();
        gunRecoil = gunStats.gunRecoil;
        gunDelay = gunStats.gunDelay;
    }

    public IEnumerator FireGunCo()
    {
        yield return new WaitForSeconds(gunDelay);
        iceBlast.SetActive(true);
        yield return new WaitForSeconds(0.3f);
        iceBlast.SetActive(false);
        GetComponentInParent<PlayerShootState>().CompleteShot();
    }
}
