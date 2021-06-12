using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ActionGraphic : MonoBehaviour
{
    [SerializeField] private float duration;

    private void OnEnable()
    {
        StartCoroutine(ClearEffectCo());
    }

    private IEnumerator ClearEffectCo()
    {
        yield return new WaitForSeconds(duration);
        Destroy(this.gameObject);
    }

}
