using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class MouseDetector : MonoBehaviour
{
    public void OnMouseEnter()
    {
        Debug.Log("Enter");
            // EventSystem.current.SetSelectedGameObject(null);
            // EventSystem.current.SetSelectedGameObject(this.gameObject);
    }
}
