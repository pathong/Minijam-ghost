using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public static class SoundManager 
{

    public static void PlaySound(AudioClip clip, Vector3 position)
    {
        GameObject soundGameObject = new GameObject("Sound");
        soundGameObject.transform.position = position;
        AudioSource audioSource = soundGameObject.AddComponent<AudioSource>();
        audioSource.spatialBlend = 1f;
        audioSource.clip = clip;
        audioSource.Play();
    }
}
