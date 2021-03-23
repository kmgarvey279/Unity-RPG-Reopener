using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class GunIcon : MonoBehaviour
{
    [SerializeField] private ActiveGun activeGun;
    [Header("Image")]
    [SerializeField] private Image currentIcon;
    [Header("Sprites")]
    [SerializeField] private Sprite regularIcon;
    [SerializeField] private Sprite iceIcon;
    [SerializeField] private Sprite flameIcon;
    [SerializeField] private Sprite elecIcon;

    private void Start()
    {
        SetIcon();   
    }

    public void SetIcon()
    {
        if(activeGun.runtimeGun == GunType.normalGun)
        {
            currentIcon.sprite = regularIcon;
        } 
        else if(activeGun.runtimeGun == GunType.flameGun)
        {
            currentIcon.sprite = flameIcon;
        }
        else if(activeGun.runtimeGun == GunType.iceGun)
        {
            currentIcon.sprite = iceIcon;
        }
        else if(activeGun.runtimeGun == GunType.elecGun)
        {
            currentIcon.sprite = elecIcon;
        }

    }
}
