using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BattleCursor : MonoBehaviour
{
    private Vector3 moveDirection;
    public Tile selectedTile;

    private void Update()
    {
        float moveX = Input.GetAxisRaw("Horizontal");
        float moveY = Input.GetAxisRaw("Vertical");
        moveDirection = new Vector3(moveX, moveY);
    }

    private void FixedUpdate()
    {
        transform.Translate(moveDirection * Time.deltaTime * 5f);
    }

    private void OnTriggerEnter2D(Collider2D other)
    {
        Debug.Log("enter");
        if(other.gameObject.CompareTag("Tile") && other.isTrigger)
        {
            selectedTile = other.gameObject.GetComponent<Tile>();
        }
    }

    private void OnTriggerExit2D(Collider2D other)
    {
        Debug.Log("exit");
        if(other.gameObject.CompareTag("Tile") && other.isTrigger)
        {
            selectedTile = null;
        }
    }
}
