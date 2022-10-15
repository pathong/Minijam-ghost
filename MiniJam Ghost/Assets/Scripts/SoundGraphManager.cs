using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SoundGraphManager : MonoBehaviour
{
    public static SoundGraphManager soundGraphManager;

    public SoundBar leftBar;
    public SoundBar rightBar;
    public SoundBar upBar;
    public SoundBar downBar;

    private void Awake()
    {
        if(soundGraphManager == null) { soundGraphManager = this; }

        
    }

    public static void TriggerSoundGraph(Vector2 dir)
    {
        soundGraphManager.GetXYBar(dir);
    }


    public void GetXYBar(Vector2 dir)
    {
        Vector2 diff = dir - (Vector2)Camera.main.transform.position;
        SoundBar Xbar = null;
        SoundBar Ybar = null;  

        // x-axis
        if(diff.x > 0)
        {
            Xbar = rightBar;
        }
        else
        {
            Xbar = leftBar;
        }


        if(diff.y > 0)
        {
            Ybar = upBar;
        }
        else
        {
            Ybar = downBar;
        }


        StartCoroutine(AmpCR(Ybar));
        StartCoroutine(AmpCR(Xbar));



    }


    IEnumerator AmpCR(SoundBar bar, float amp = .01f )
    {
        bar.amplitude = amp;
        yield return new WaitForSeconds(.5f);
        //bar.amplitude = .001f;
        StartCoroutine(FadeAmp(bar));
    }


    IEnumerator FadeAmp(SoundBar bar)
    {
        while(bar.amplitude > .001f)
        {
            bar.amplitude -= .003f;
            yield return new WaitForSeconds(.2f);
        }
        bar.amplitude = .001f;
    }


}
