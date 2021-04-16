using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Pathfinding;

public class Pathfinder : MonoBehaviour
{
    public AIPath aiPath;
    public Animator animator;
    // Start is called before the first frame update
    void Start()
    {
        animator = GetComponent<Animator>();
    }

    // Update is called once per frame
    void Update()
    {
        if(!Mathf.Approximately(aiPath.targetDirection.x, 0.0f) || !Mathf.Approximately(aiPath.targetDirection.y, 0.0f))
        {
            animator.SetFloat("Look X", Mathf.Round(aiPath.targetDirection.x));
            animator.SetFloat("Look Y", Mathf.Round(aiPath.targetDirection.y));
        }
        Vector3 velocity = aiPath.CalculateVelocity(transform.position);
        animator.SetFloat("Speed", velocity.sqrMagnitude);
    }
}
