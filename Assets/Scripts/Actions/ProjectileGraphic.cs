using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ProjectileGraphic : ActionGraphic
{
    private Vector3 destination;
    [SerializeField] private float speed;

    // Update is called once per frame
    void Update()
    {
        if(destination != null)
        {
            if(transform.position == destination)
            {
                // onTriggerActionEffect.Raise();
                Destroy(this.gameObject);
            }
            else
            {
                Vector3 moveDirection = (destination - transform.position).normalized;
                float step =  speed * Time.deltaTime; 
                transform.position = Vector3.MoveTowards(transform.position, destination, step); 
            }   
        }
    }
}
