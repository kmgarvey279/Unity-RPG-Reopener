using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using StateMachineNamespace;

[System.Serializable]
public class PlayerShootState : PlayerState
{
    [Header("Current Weapon")]
    [SerializeField] private ActiveGun activeGun;
    [Header("Shoot Animation")]
    private bool followUp = false;

    public override void OnEnter()
    {
        nextState = "";
        character.animator.SetBool("Shooting", true);
        Shoot();
    }

    public override void StateUpdate()
    {
        HandleInputs();
    }

    public override string CheckConditions()
    {   
        return nextState;
    }

    public override void OnExit()
    {
        character.animator.SetBool("Shooting", false);
    }

    void HandleInputs()
    {
        if(Input.GetButtonDown("Shoot")) 
        {
            followUp = true;
        }
    }

    private void Shoot()
    {
        character.rigidbody.velocity = Vector3.zero;
        if(activeGun.runtimeGun == GunType.normalGun)
        {
            StartCoroutine(GetComponentInChildren<NormalGunManager>().FireGunCo());
        }
        else if(activeGun.runtimeGun == GunType.iceGun)
        {
            StartCoroutine(GetComponentInChildren<IceGunManager>().FireGunCo());
        }
        else if(activeGun.runtimeGun == GunType.flameGun)
        {
           StartCoroutine(GetComponentInChildren<FlameGunManager>().FireGunCo());
        }
        else if(activeGun.runtimeGun == GunType.elecGun)
        {
            StartCoroutine(GetComponentInChildren<ElecGunManager>().FireGunCo());
        }
    }

    public void CompleteShot()
    {
        if(followUp)
        {
            followUp = false;
            Shoot();
        } 
        else 
        {
            nextState = "MoveState";
        }
    }
}
