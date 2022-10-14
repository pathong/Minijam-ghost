using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System;


public abstract class BulletBehaviour : MonoBehaviour
{

    public float lifeTime;
    [SerializeField] private float bulletSpeed;
    private Rigidbody2D rb;

    private void Awake()
    {
        rb = GetComponent<Rigidbody2D>();
        lifeTime = UnityEngine.Random.Range(.3f, .5f);
        rb.AddForce(transform.right * bulletSpeed*10);
    }


    private void Update()
    {
        if(lifeTime >= 0)
        {
            lifeTime -= Time.deltaTime;
        }
        else
        {
            InvokeBulletFunction();
        }
    }


    private void OnTriggerEnter2D(Collider2D collision)
    {
        InvokeBulletFunction();
    }


    public virtual void InvokeBulletFunction()
    {
        Debug.Log("invoke bullet function");
        Destroy(gameObject);
    }


}
