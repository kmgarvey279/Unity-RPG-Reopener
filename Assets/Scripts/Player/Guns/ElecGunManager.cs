using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ElecGunManager : GunFireManager
{
    [SerializeField] private GetObjectsInRange getObjects;
    private List<GameObject> targetsInRange = new List<GameObject>();
    [SerializeField] private LayerMask ignoreLayer;
    [SerializeField] private GameObject lightningPrefab; 
    [SerializeField] private float lightningDuration = 10f;

    private void Start()
    {
        playerRB = GetComponentInParent<Rigidbody2D>(); 
        playerAnimator = GetComponentInParent<Animator>();
        gunRecoil = gunStats.gunRecoil;
        gunDelay = gunStats.gunDelay;
    }

    public IEnumerator FireGunCo()
    {
        // lightningRange.SetActive(true);
        Vector3 lightningDirection = new Vector3();
        targetsInRange = getObjects.GetObjectList();
        if(targetsInRange.Count > 0)
        {
            Transform target = FindNearestTarget();
            lightningDirection = target.position - firePoint.position;
        } 
        else
        {
            lightningDirection = new Vector3(playerAnimator.GetFloat("Look X"), playerAnimator.GetFloat("Look Y"));
        } 
        yield return new WaitForSeconds(gunDelay);
        RaycastHit2D hit = Physics2D.Raycast(firePoint.position, lightningDirection, 10f, ~ignoreLayer);
        if(hit.collider != null)
        {
            Debug.Log(hit.collider.gameObject.tag);
            if(hit.collider.gameObject.CompareTag("Enemy"))
            {
                Debug.Log("hit enemy");
            }
            else
            {
                Debug.Log("hit something else");
            }
        }
        Debug.DrawRay(transform.position, lightningDirection * 10f, Color.blue, 5);
        // GameObject lightningObject = Instantiate(lightningPrefab, firePoint.position, Quaternion.identity);
        // float angle = Mathf.Atan2(lightningDirection.x, lightningDirection.y) * Mathf.Rad2Deg;
        // lightningObject.transform.rotation = Quaternion.Euler(0, 0, angle);
        yield return null;
        // Destroy(lightningObject);
        GetComponentInParent<PlayerShootState>().CompleteShot();
    }

    private Transform FindNearestTarget()
    {
        float shortestDistance = Mathf.Infinity;
        GameObject nearestTarget = null;
        foreach (GameObject target in targetsInRange)
        {
            float distance = Vector3.Distance(target.transform.position, gameObject.GetComponentInParent<Transform>().position);
            if(distance < shortestDistance)
            {
                shortestDistance = distance;
                nearestTarget = target;
            }
        }
        return nearestTarget.transform;
    }
}
