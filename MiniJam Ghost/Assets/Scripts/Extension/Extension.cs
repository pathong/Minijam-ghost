using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public static class Extension 
{
    public static Transform GetPlayer()
    {
        if(GameObject.FindGameObjectWithTag("Player") != null)
        {
            return GameObject.FindGameObjectWithTag("Player").transform;
        }
        else
        {
            return null;
        }
    }
}
