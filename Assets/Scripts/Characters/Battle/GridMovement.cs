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
    [SerializeField] private SignalSenderGO onMoveComplete;
    [SerializeField] private SignalSenderGO onKnockbackComplete;

    private void Start()
    {
        combatant = GetComponentInParent<Combatant>();
    }

    public void Move(List<Tile> path, MovementType movementType)
    {
        this.path = path;
        if(path.Count <= 1 )
        {
            onMoveComplete.Raise(combatant.gameObject);
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
        if(movementType == MovementType.Move)
        {
            combatant.animator.SetTrigger("Idle");
            onMoveComplete.Raise(combatant.gameObject);
        }
        else if(movementType == MovementType.Knockback)
        {
            onKnockbackComplete.Raise(combatant.gameObject);
        }
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