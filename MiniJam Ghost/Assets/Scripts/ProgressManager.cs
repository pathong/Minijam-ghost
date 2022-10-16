using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ProgressManager : MonoBehaviour
{
    public static ProgressManager i;
    public string QuestName;
    public int progress;
    public int currentProgress;
    public AudioClip progressSound;
    public System.Action OnProgressActivate;

    private void Awake()
    {
        if(i == null) { i = this; }
        else { Destroy(this); }
    }


    public void IncreaseProgress()
    {
        currentProgress++;
        SoundManager.PlaySound(progressSound, Vector2.zero);
        OnProgressActivate?.Invoke();

        if(currentProgress >= progress)
        {
            InGameMenu.instance.OnPlayerFinishHanddler();
        }
    }










}

