using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ProgressManager : MonoBehaviour
{
    public static ProgressManager i;

    public Progress[] progresses;
    private Progress current;



    public int MaxProgress;
    public int currProgress;

    public AudioClip progressSound;
    public ProgressUI progressUI;

    private void Awake()
    {
        if(i == null) { i = this; }
        else { Destroy(this); }

        currProgress = 0;
        current = GetCurrentProgress();
        progressUI.SetData(current.Description, currProgress, MaxProgress);
    }





    public void IncreaseProgress()
    {
        currProgress++;
        SoundManager.PlaySound(progressSound, Vector2.zero);

        current = GetCurrentProgress(); 

        progressUI.SetData(current.Description, currProgress, MaxProgress);
        if (currProgress >= MaxProgress)
        {
            InGameMenu.instance.OnPlayerFinishHanddler();
        }

    }

    Progress GetCurrentProgress()
    {
        Progress pro = progresses[0];
        foreach (Progress progress in progresses)
        {
            if(currProgress >= progress.startAt)
            {
                pro = progress;
            }

        }
        return pro;
        
    }


}


[System.Serializable]
public struct Progress{

    [TextArea]
    public string Description;

    public int startAt;


}

