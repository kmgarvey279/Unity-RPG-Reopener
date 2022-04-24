using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ActionPointDisplay : MonoBehaviour
{
    private List<GameObject> icons = new List<GameObject>();
    [SerializeField] private GameObject iconPrefab;


    public void DisplayAP(int apCount)
    {
        if(icons.Count > 0)
        {
            for(int i = 0; i < icons.Count; i++)
            {
                Destroy(icons[i]);
            }
            icons.Clear();
        }
        for(int i = 0; i < apCount; i++)
        {
            GameObject apIcon = Instantiate(iconPrefab, transform.position, Quaternion.identity);
            apIcon.transform.parent = gameObject.transform;      
            icons.Add(apIcon);  
        }
    }

    public void ShowPreview(int apCost)
    {
        for(int i = 0; i < apCost; i++)
        {
            if(i < icons.Count)
            {
                icons[i].GetComponent<Animator>().SetBool("Flashing", true);
            }
        }
    }

    public void HidePreview()
    {
        if(icons.Count > 0)
        {
            for(int i = 0; i < icons.Count; i++)
            {
                icons[i].GetComponent<Animator>().SetBool("Flashing", false);
            }
        }
    }
}
