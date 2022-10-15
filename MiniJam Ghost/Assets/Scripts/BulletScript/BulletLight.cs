using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BulletLight : BulletBehaviour 
{
    [SerializeField] private GameObject lightBulb;


    public override void InvokeBulletFunction()
    {
        
        base.InvokeBulletFunction();
        GameObject light = Instantiate(lightBulb, transform.position, Quaternion.identity);
        Destroy(light, 10f);
        
    }

}
