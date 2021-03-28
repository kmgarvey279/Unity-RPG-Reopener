using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class MoveManager : MonoBehaviour
{
    [HideInInspector] public Character character;
    [HideInInspector] public Vector3 moveDirection;

    // Start is called before the first frame update
    public virtual void Start()
    {        
        character = GetComponentInParent<Character>();
        moveDirection = new Vector3(0,0,0);
    }

    public virtual void HandleMoveLogic()
    {    
    }

    public void HandleMovePhysics()
    {
        float speedTemp = character.characterInfo.moveSpeed.GetValue();
        if(character.lookDirection.x != moveDirection.x && character.lookDirection.y != moveDirection.y)
        {
            speedTemp = speedTemp / 2f;
        }
        else if(!Mathf.Approximately(moveDirection.x, 0.0f) || !Mathf.Approximately(moveDirection.y, 0.0f))
        {
            character.ChangeLookDirection(moveDirection);
        }
        character.rigidbody.velocity = new Vector3(moveDirection.x * speedTemp, moveDirection.y * speedTemp);
        UpdateAnimator();
    }

    public void UpdateAnimator()    
    {    
        float tempX = Mathf.Round(character.lookDirection.x);
        float tempY = Mathf.Round(character.lookDirection.y);
        character.animator.SetFloat("Look X", tempX);
        character.animator.SetFloat("Look Y", tempY);
        character.animator.SetFloat("Speed", moveDirection.sqrMagnitude);
    }
}
