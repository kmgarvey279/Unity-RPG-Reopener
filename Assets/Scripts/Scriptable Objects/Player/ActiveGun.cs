using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public enum GunType
{
    normalGun,
    elecGun,
    iceGun,
    flameGun
}

[System.Serializable]
[CreateAssetMenu]
public class ActiveGun : ScriptableObject
{
    public GunType runtimeGun;

    private void Start()
    {
        runtimeGun = GunType.normalGun;
    }
}
