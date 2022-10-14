using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Enemy : MonoBehaviour
{
    public Rigidbody2D rb;
    public GameObject player;
    public float moveSpeed;
    public Vector3 direction;
    public Vector3 localScale;

    // Start is called before the first frame update
    void Awake()
    {
        
    }

    // Update is called once per frame
    void FixedUpdate()
    {
        MoveEnemy(player.transform.position);
    }

    void MoveEnemy(Vector3 targetPosition)
    {
        direction = (targetPosition - transform.position).normalized;
        rb.velocity = new Vector2(direction.x, direction.y) * moveSpeed;
    }
}
