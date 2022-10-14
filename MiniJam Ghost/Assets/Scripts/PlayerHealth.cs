using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerHealth : MonoBehaviour
{
    public int Health;
    private int currentHealth;

    private void OnEnable()
    {
        currentHealth = Health;
    }


    public void TakeDamge()
    {
        currentHealth -= 1;

    }
}
