using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System;


public abstract class BulletBehaviour : MonoBehaviour
{

    [SerializeField] private float bulletSpeed = 250f;
    private Rigidbody2D rb;
    private float bulletDistance;

    private void Awake()
    {
        rb = GetComponent<Rigidbody2D>();
        rb.AddForce(transform.right * bulletSpeed *10);
    }


    private void Update()
    {

        if (Vector2.Distance(transform.position, GameObject.FindGameObjectWithTag("Player").transform.position) >= bulletDistance)
        {
            InvokeBulletFunction();
        }
    }


    private void OnTriggerEnter2D(Collider2D collision)
    {
        InvokeBulletFunction();
        IDamangable damagable = collision.gameObject.GetComponent<IDamangable>();
        if(damagable != null)
        {
            damagable.TakeDamage();

            // knock back

            Vector2 dir = collision.transform.position - transform.position;
            collision.GetComponent<Rigidbody2D>()?.AddForce(dir.normalized, ForceMode2D.Impulse);

        }
    }


    public virtual void InvokeBulletFunction()
    {
        Debug.Log("invoke bullet function");
        Destroy(gameObject);
    }

    public void SetDistance(float distance)
    {
        bulletDistance = distance;
    }


}
