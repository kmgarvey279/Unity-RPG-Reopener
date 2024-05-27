using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Interactable : MonoBehaviour
{
    [field: SerializeField] public bool FaceOnInteract { get; private set; }
    
    public virtual void Interact()
    {  
    }
}
