using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ImpactTrigger : MonoBehaviour
{
    public GameObject explosionPrefab;

    private void OnCollisionEnter2D(Collision2D other)
    {
        Destroy(this.gameObject);
        Instantiate(explosionPrefab, transform.position, Quaternion.identity);
    }
}
