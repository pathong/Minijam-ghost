using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerHealth : MonoBehaviour, IDamangable
{
    public int Health;
    private int currentHealth;

    private void OnEnable()
    {
        currentHealth = Health;
    }

    void IDamangable.TakeDamage()
    {
        Debug.Log("ouch!");
        currentHealth -= 1;
        if(currentHealth < 0) { Destroy(gameObject, 1f); }
    }
}
