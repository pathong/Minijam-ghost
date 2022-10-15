using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class EnemyTest : Enemy 
{
    [SerializeField] private LayerMask playerLayer;
    protected override void Attack()
    {
        base.Attack();

        Collider2D[] hits = Physics2D.OverlapCircleAll(transform.position, attackRange, playerLayer);
        foreach (var col in hits)
        {
            Debug.Log(col.name);
            IDamangable damagable = col.GetComponent<IDamangable>();
            if(damagable != null)
            {
                damagable.TakeDamage();
            }
        }
    }
}

