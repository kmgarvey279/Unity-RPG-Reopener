using Pathfinding;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using static OverworldEnemy;

public class NPCMovement : MonoBehaviour
{
    [SerializeField] private Transform startPoint;
    [SerializeField] private GameObject target;

    private readonly float wanderSpeed = 3f;
    private readonly float moveToDestinationSpeed = 5f;
    private readonly float chaseSpeed = 8f;

    [SerializeField] private Animator animator;
    private AIPath aiPath;
    private AIDestinationSetter setter;
    private Seeker seeker;

    public enum NPCMoveState
    {
        Stationery,
        Wander,
        Relocate,
        Chase
    }

    [SerializeField] public NPCMoveState defaultState;
    [field: SerializeField] public NPCMoveState CurrentState { get; private set; }

    public void Start()
    {
        aiPath = GetComponent<AIPath>();
        setter = GetComponent<AIDestinationSetter>();
        seeker = GetComponent<Seeker>();

        SwitchToDefaultState();
    }

    public void Update()
    {
        if (CurrentState != NPCMoveState.Stationery && aiPath.canMove)
        {
            if (!Mathf.Approximately(aiPath.desiredVelocity.x, 0.0f) || !Mathf.Approximately(aiPath.desiredVelocity.y, 0.0f))
            {
                SetDirection(aiPath.desiredVelocity.normalized);
            }

            if (CurrentState == NPCMoveState.Relocate && aiPath.reachedEndOfPath)
            {
                SwitchToDefaultState();
            }
            else if (CurrentState == NPCMoveState.Wander && (aiPath.reachedEndOfPath || !aiPath.hasPath))
            {
                StartCoroutine(WanderCo());
            }
        }
    }

    public void SetDirection(Vector2 newDirection)
    {
        animator.SetFloat("Look X", newDirection.x);
        animator.SetFloat("Look Y", newDirection.y);
    }

    //default state only changes behavior if set to wander, everything else is treated as stationary
    private void SwitchToDefaultState()
    {
        if (defaultState == NPCMoveState.Wander)
        {
            SwitchToWanderState();
        }
        else
        {
            SwitchToStationaryState();
        }
    }

    private IEnumerator WanderCo()
    {
        float delay = Random.Range(0.5f, 2f);
        yield return new WaitForSeconds(delay);

        aiPath.destination = GetRandomPoint(startPoint.position, 0.5f, 6f);
        aiPath.SearchPath();
    }

    //private IEnumerator GiveUpCo()
    //{
    //    yield return new WaitForSeconds(1f);
    //    if (!IsTargetReachable())
    //    {
    //        ReturnToStartPoint();
    //    }
    //}

    public Vector2 GetRandomPoint(Vector2 origin, float minRadius, float maxRadius)
    {
        Vector2 randomDirection = Random.insideUnitCircle.normalized; 
        float minRadius2 = minRadius * minRadius;
        float maxRadius2 = maxRadius * maxRadius;
        float randomDistance = Mathf.Sqrt(Random.value * (maxRadius2 - minRadius2) + minRadius2);
        return origin + randomDirection * randomDistance;
    }

    public void SwitchToStationaryState()
    {
        if (setter.target != null)
        {
            setter.target = null;
        }

        CurrentState = NPCMoveState.Stationery;
        aiPath.maxSpeed = 0;
    }

    public void SwitchToWanderState()
    {
        if (setter.target != null)
        {
            setter.target = null;
        }

        CurrentState = NPCMoveState.Wander;
        aiPath.maxSpeed = wanderSpeed;
    }

    public void ReturnToStartPoint()
    {
        SwitchToRelocateState(startPoint.position);
    }


    public void SwitchToRelocateState(Vector2 destination)
    {
        if (setter.target != null)
        {
            setter.target = null;
        }

        CurrentState = NPCMoveState.Relocate;

        aiPath.destination = destination;
        aiPath.maxSpeed = moveToDestinationSpeed;

        aiPath.SearchPath();
    }

    public void SwitchToChaseState(Transform target)
    {
        //Debug.Log("Target Reachable: " + IsTargetReachable(target.transform));
        if (target)
        {
            CurrentState = NPCMoveState.Chase;

            aiPath.maxSpeed = chaseSpeed;

            setter.target = target;
        }

        StartCoroutine(StuckCheckCo());
    }

    private IEnumerator StuckCheckCo()
    {
        Vector2 lastPosition = transform.position;
        int stuckCounter = 0;

        while (CurrentState == NPCMoveState.Chase && setter.target != null)
        {
            if (!IsTargetReachable(setter.target))
            {
                stuckCounter++;
                if (stuckCounter > 200)
                {
                    stuckCounter = 0;
                    ReturnToStartPoint();
                }

                if (aiPath.hasPath && aiPath.reachedEndOfPath)
                {
                    aiPath.canSearch = false;
                    //aiPath.canMove = false;
                }
            }
            else 
            {
                //aiPath.canMove = true;
                aiPath.canSearch = true;
                stuckCounter = 0;
                lastPosition = transform.position;
            }
            Debug.Log("Stuck counter: " + stuckCounter);
            yield return new WaitForEndOfFrame();
        }
        aiPath.canSearch = true;
    }

    public bool IsTargetReachable(Transform target)
    {
        GraphNode node1 = (GraphNode)AstarPath.active.GetNearest(transform.position, NNConstraint.Default);
        GraphNode node2 = (GraphNode)AstarPath.active.GetNearest(target.position, NNConstraint.Default);

        return PathUtilities.IsPathPossible(node1, node2);
    }

    public void LockMovement(bool isLocked)
    {
        aiPath.canMove = !isLocked;
    }

}
