using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public static class SoundManager 
{

    public static void PlaySound(AudioClip clip, Vector3? position = null)
    {
        if(position == null) { position = Camera.main.transform.position; }
        GameObject soundGameObject = new GameObject("Sound");
        soundGameObject.transform.position = (Vector2)position;
        AudioSource audioSource = soundGameObject.AddComponent<AudioSource>();
        audioSource.spatialBlend = 1f;
        audioSource.clip = clip;
        audioSource.Play();
    }
}
