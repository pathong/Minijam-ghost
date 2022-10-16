using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerHealth : MonoBehaviour, IDamangable
{
    public int Health;
    [HideInInspector] public int currentHealth;

    [SerializeField] private AudioClip TakeDmgSound;

    private void OnEnable()
    {
        currentHealth = Health;
    }



    void IDamangable.TakeDamage()
    {
        if(currentHealth <= 0) { return; }
        SoundManager.PlaySound(TakeDmgSound,transform.position);
        currentHealth -= 1;
        if(currentHealth <= 0) { Destroy(gameObject); }
    }
}
