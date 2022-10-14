using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.InputSystem;

public class PlayerMovement : MonoBehaviour
{
    [SerializeField] private float _moveSpeed;
    [SerializeField] private Rigidbody2D rb;
    private Vector2 _dir;


    private void Awake()
    {
        PlayerAction playerAction = new PlayerAction();
        playerAction.Enable();
        playerAction.Movement.Move.performed += ctx => _dir = ctx.ReadValue<Vector2>();
        playerAction.Movement.Move.canceled += _ => _dir = Vector2.zero;
    }

    private void FixedUpdate()
    {
        rb.velocity = _dir * _moveSpeed * Time.deltaTime;
        
    }

}