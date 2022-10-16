using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

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

        UnityEngine.Object.Destroy(soundGameObject, 2);
    }

    public static void PlaySound(AudioClip clip, Vector3? position = null, float volume = 1f)
    {
        if(position == null) { position = Camera.main.transform.position; }
        GameObject soundGameObject = new GameObject("Sound");
        soundGameObject.transform.position = (Vector2)position;
        AudioSource audioSource = soundGameObject.AddComponent<AudioSource>();
        audioSource.spatialBlend = 1f;
        audioSource.volume = volume;
        audioSource.clip = clip;
        audioSource.Play();

        UnityEngine.Object.Destroy(soundGameObject, 2);
    }
}
