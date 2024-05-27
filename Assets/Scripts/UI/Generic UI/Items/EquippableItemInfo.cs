using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;

public class EquippableItemInfo : MonoBehaviour
{
    [Header("Header")]
    [SerializeField] private GameObject header;
    [SerializeField] private TextMeshProUGUI itemNameValue;
    [Header("Textbox")]
    [SerializeField] private GameObject textbox;
    [SerializeField] private GameObject textPrefab;
    private List<GameObject> textObjects = new List<GameObject>();

    // Start is called before the first frame update
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        
    }
}
