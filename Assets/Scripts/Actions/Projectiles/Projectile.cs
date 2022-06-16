using UnityEngine;

public class Projectile : MonoBehaviour
{
    private bool moving;
    private Vector3 target;
    private float moveSpeed = 5f;
    public bool reachedTarget = false;

    private void Update()
    {
        if(moving)
        {
            if(Vector3.Distance(transform.position, target) < 0.0001f)
            {
                reachedTarget = true;
            }
            else
            {
                float step = moveSpeed * Time.deltaTime; 
                transform.position = Vector3.MoveTowards(transform.position, target, step);   
            }
        }
    }

    public void Move(Vector3 target)
    {
        this.target = target;
        moving = true;
    }
}
