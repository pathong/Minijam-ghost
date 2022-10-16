using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class FireField : MonoBehaviour
{
    [SerializeField] private float radius;
    [SerializeField] private float timePeriod;
    [SerializeField] private float lifeTime;
    [SerializeField] private LayerMask enemyLayer;



    IEnumerator waitToDestroy()
    {
        yield return new WaitForSeconds(lifeTime);
        Destroy(gameObject);
    }

    IEnumerator DoDamage()
    {
        while (true)
        {
            yield return new WaitForSeconds(timePeriod);
            Collider2D[] hits = Physics2D.OverlapCircleAll(transform.position, radius, enemyLayer);
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






}
