using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerHealth : MonoBehaviour, IDamangable
{
    public int Health;
    [HideInInspector] public int currentHealth;

    [SerializeField] private AudioClip TakeDmgSound;

    [SerializeField] private float healTime;
    [SerializeField] private float currentHealTime;


    private void OnEnable()
    {
        currentHealth = Health;
        currentHealTime = healTime;
    }


    private void Update()
    {
        if(currentHealth < Health)
        {
            if(currentHealTime>= 0)
            {
                currentHealTime-= Time.deltaTime;
            }
            else
            { 
                currentHealTime = healTime;
                currentHealth++;
            }
        }
    }



    void IDamangable.TakeDamage()
    {
        if(currentHealth <= 0) { return; }
        SoundManager.PlaySound(TakeDmgSound,transform.position);
        currentHealth -= 1;
        currentHealTime = healTime;
        if(currentHealth <= 0) {
            InGameMenu.instance.OnPlayerDeadHanddler();
            
        }
    }



}
