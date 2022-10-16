using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SoundUtil
{
    public static AudioClip RandSound(AudioClip[] sounds) {
        int n = Random.Range(0, sounds.Length);
        return sounds[n];
    }
}
