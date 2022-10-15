using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.InputSystem;

public class PlayerMovement : MonoBehaviour
{
    public Animator animator;
    [SerializeField] private GameObject sprite;
    [SerializeField] private float _moveSpeed;
    [SerializeField] private AudioClip walkSound;
    [SerializeField] private SpriteRenderer gunRend;
    private bool isWalking;
    private bool isFacingRight = true;
    public float MoveSpeed
    {
        get => _moveSpeed;
        set => _moveSpeed = value;
    }
    
    [SerializeField] private Rigidbody2D rb;
    private Vector2 _dir;

    [SerializeField] private float _knockbackForce;


    private void Awake()
    {
        PlayerAction playerAction = new PlayerAction();
        playerAction.Enable();
        playerAction.Movement.Move.performed += ctx => _dir = ctx.ReadValue<Vector2>();
        playerAction.Movement.Move.canceled += _ => _dir = Vector2.zero;
        isWalking = false;
    }

    private void FixedUpdate()
    {
        rb.velocity = _dir * _moveSpeed * Time.deltaTime;
        animator.SetFloat("SpeedY", _dir.y);

        if(_dir.x < 0 && isFacingRight)
        {
            sprite.transform.localRotation = Quaternion.Euler(0, 180, 0);
            isFacingRight = false;
        }
        if(_dir.x > 0 && !isFacingRight)
        {
            sprite.transform.localRotation = Quaternion.Euler(0, 0, 0);
            isFacingRight = true;
        }
        if(_dir.y < 0)
        {
            gunRend.sortingOrder = 3;
        }
        if(_dir.y > 0)
        {
            gunRend.sortingOrder = 1;
        }

        if(!isWalking) StartCoroutine(WalkSound());
    }


    public void Knockback(Vector2 knockbackDir)
    {
        rb.AddForce(knockbackDir.normalized, ForceMode2D.Impulse);
    }
    IEnumerator WalkSound()
    {
        if (isWalking) { yield return null; }
        while(rb.velocity.magnitude > 0f)
        {
            isWalking = true;
            float wait = 1f;
            SoundManager.PlaySound(walkSound, transform.position);
            yield return new WaitForSeconds(wait);
            
        }
        if(rb.velocity.magnitude <= 0f)
        {
            isWalking=false;
        }
    }

}
