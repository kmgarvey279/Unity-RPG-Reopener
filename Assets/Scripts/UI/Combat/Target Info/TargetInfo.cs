using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class TargetInfo : MonoBehaviour
{
    public GameObject display;

    public void Display(bool show)
    {
        display.SetActive(show);
    }
}
