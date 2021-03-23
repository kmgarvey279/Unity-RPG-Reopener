using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class FlameGunManager : GunFireManager
{
    [SerializeField] private GameObject explosivePrefab;
    
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
        // if(GetComponentInParent<StateMachine>().currentState ===)
        // {
            GameObject projectileObject = Instantiate(explosivePrefab, firePoint.position, Quaternion.identity);
            Projectile projectile = projectileObject.GetComponent<Projectile>();
            Vector3 projectileDirection = new Vector3(playerAnimator.GetFloat("Look X"), playerAnimator.GetFloat("Look Y"));
            projectile.Launch(projectileDirection, characterStats);
            Vector3 recoilVector = new Vector3((projectileDirection.x * -1.0f) * gunRecoil, (projectileDirection.y * -1.0f) * gunRecoil);
            playerRB.AddForce(recoilVector);
        // }
        yield return null;
        GetComponentInParent<PlayerShootState>().CompleteShot();
    }
}
