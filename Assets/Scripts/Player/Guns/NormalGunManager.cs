using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class NormalGunManager : GunFireManager
{
    [SerializeField] private GameObject projectilePrefab;

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
        GameObject projectileObject = Instantiate(projectilePrefab, firePoint.position, Quaternion.identity);
        Projectile projectile = projectileObject.GetComponent<Projectile>();
        Vector3 projectileDirection = new Vector3(playerAnimator.GetFloat("Look X"), playerAnimator.GetFloat("Look Y"));
        projectile.Launch(projectileDirection);
        Vector3 recoilVector = new Vector3((projectileDirection.x * -1.0f) * gunRecoil, (projectileDirection.y * -1.0f) * gunRecoil);
        playerRB.AddForce(recoilVector);
        yield return null;
        GetComponentInParent<PlayerShootState>().CompleteShot();
    }
}
