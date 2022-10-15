using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BulletFire : BulletBehaviour 
{
    [SerializeField] private GameObject fireField;
    public override void InvokeBulletFunction()
    {
        base.InvokeBulletFunction();
        // Spawn fire
        Instantiate(fireField, transform.position, Quaternion.identity);
    }

}
