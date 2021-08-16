using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class AnimatedMask : MonoBehaviour
{
    [SerializeField] private SpriteMask mask;
    [SerializeField] private SpriteRenderer targetRenderer;

    private void LateUpdate() 
    {
        if(mask.sprite != targetRenderer.sprite)
        {
            mask.sprite = targetRenderer.sprite;
        }
    }
}
