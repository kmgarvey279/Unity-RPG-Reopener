using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public enum MovementType
{
    Move,
    Knockback
}

public class GridMovement : MonoBehaviour
{
    private Combatant combatant; 
    [SerializeField] private List<Tile> path = new List<Tile>();
    private MovementType movementType;
    public bool moveComplete;

    private void Start()
    {
        combatant = GetComponentInParent<Combatant>();
    }

    public void Move(List<Tile> path, MovementType movementType)
    {
        moveComplete = false;
        this.path = path;
        if(path.Count <= 1 )
        {
            moveComplete = true;
        }
        else
        {
            this.movementType = movementType;
            if(movementType == MovementType.Move)
            {
                combatant.animator.SetTrigger("Move");
            }
        }
    }

    public virtual void EndMove()
    {
        combatant.animator.SetTrigger("Idle");
        moveComplete = true;
        if(combatant is PlayableCombatant)
        {
            combatant.SetDirection(new Vector2(1,0));
        }
        else if(combatant is EnemyCombatant)
        {
            combatant.SetDirection(new Vector2(-1,0));
        }
    }

    private void Update()
    {
        float moveSpeed = 5f;
        if(movementType == MovementType.Move)
        {
            moveSpeed = 4f;
        }
        if(path.Count > 0)
        {
            if(Vector3.Distance(combatant.transform.position, path[0].transform.position) < 0.0001f)
            {
                path.RemoveAt(0);
                if(path.Count == 0)
                {
                    EndMove();
                }
            }
            else
            {
                if(movementType == MovementType.Move)
                {
                    Vector3 moveDirection = (path[0].transform.position - combatant.transform.position).normalized;
                    if(!Mathf.Approximately(moveDirection.x, 0.0f) || !Mathf.Approximately(moveDirection.y, 0.0f))
                    {
                        combatant.SetDirection(new Vector2(Mathf.Round(moveDirection.x), Mathf.Round(moveDirection.y)));
                    }
                }
                float step = moveSpeed * Time.deltaTime; 
                combatant.transform.position = Vector3.MoveTowards(combatant.transform.position, path[0].transform.position, step);   
            }
        }
    }

}