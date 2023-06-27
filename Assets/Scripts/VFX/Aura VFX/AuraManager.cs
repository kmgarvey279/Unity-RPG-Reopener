using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class AuraManager : MonoBehaviour
{
    private Dictionary <GameObject, GameObject> auras = new Dictionary<GameObject, GameObject>();

    public void AddAura(GameObject auraPrefab)
    {
        if (!auras.ContainsKey(auraPrefab))
        {
            GameObject auraObject = Instantiate(auraPrefab, transform.position, Quaternion.identity);
            auraObject.transform.parent = transform;

            auras.Add(auraPrefab, auraObject);
        }
    }

    public void StartTimeStop()
    {
        foreach (KeyValuePair<GameObject, GameObject> entry in auras)
        {
            entry.Value.GetComponent<AuraVFXParent>().StartTimeStop();
        }
    }

    public void EndTimeStop()
    {
        foreach (KeyValuePair<GameObject, GameObject> entry in auras)
        {
            entry.Value.GetComponent<AuraVFXParent>().EndTimeStop();
        }
    }

    public void HideAura(GameObject auraPrefab)
    {
        auras[auraPrefab].SetActive(false);
    }

    public void UnhideAura(GameObject auraPrefab)
    {
        auras[auraPrefab].SetActive(true);
    }

    public void RemoveAura(GameObject auraPrefab)
    {
        Destroy(auras[auraPrefab]);
        auras.Remove(auraPrefab);
    }
}
