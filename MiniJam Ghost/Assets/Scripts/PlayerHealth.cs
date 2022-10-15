using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerHealth : MonoBehaviour, IDamangable
{
    public int Health;
    private int currentHealth;

    [SerializeField] private AudioClip TakeDmgSound;

    private void OnEnable()
    {
        currentHealth = Health;
    }



    void IDamangable.TakeDamage()
    {
        SoundManager.PlaySound(TakeDmgSound);
        currentHealth -= 1;
        if(currentHealth < 0) { Destroy(gameObject, 1f); }
    }
}
