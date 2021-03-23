using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class GunSelectManager : MonoBehaviour
{
    [SerializeField] private ActiveGun activeGun;
    private GunType nextGun;
    [SerializeField] private SignalSender changeGunSignal;
    private float inputPositiveThreshold = 0.75f;
    private float inputNegativeThreshold = -0.75f;

    // Start is called before the first frame update
    void Start()
    {
        nextGun = activeGun.runtimeGun;
    }

    // Update is called once per frame
    void Update()
    {
        float inputX = Input.GetAxis("Change Weapon X");
        float inputY = Input.GetAxis("Change Weapon Y");
        if(!Mathf.Approximately(inputX, 0.0f) || !Mathf.Approximately(inputY, 0.0f))
        {
            if(inputX > inputPositiveThreshold)
            {
                inputX = 0;
                inputY = 0;
                nextGun = GunType.elecGun;
            }
            else if(inputX < inputNegativeThreshold)
            {
                inputX = 0;
                inputY = 0;
                nextGun = GunType.flameGun;
            }
            else if(inputY > inputPositiveThreshold)
            {
                inputX = 0;
                inputY = 0;
                nextGun = GunType.iceGun;
            }
            else if(inputY < inputNegativeThreshold)
            {
                inputX = 0;
                inputY = 0;
                nextGun = GunType.normalGun;
            }
        }
    }

    private void FixedUpdate()
    {
        if(nextGun != activeGun.runtimeGun)
        {
            activeGun.runtimeGun = nextGun;
            changeGunSignal.Raise();
        }
    }
}
