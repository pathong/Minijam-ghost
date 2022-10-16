using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Experimental.Rendering.Universal;

[RequireComponent(typeof(Light2D))]
public class Lightning : MonoBehaviour
{
    // makeshift singleton
    static Lightning lightning;

    public Color dColor;
    public float dTime;
    public float dIntensity;
    //public float fadeSpeed;

    // reference
    new Light2D light;

    void Awake() {
        // singleton
        if (lightning == null) lightning = this;

        light = GetComponent<Light2D>();
    }

    void Start() {
        light.intensity = 0;;
    }

    void Update() {
        if (Input.GetKeyDown(KeyCode.L)) Trigger();
    }

    public static void Trigger() {
        if (lightning == null) return;
        lightning.RunTriggerCR();
    }

    public static void Trigger(float time) {
        if (lightning == null) return;
        lightning.RunTriggerCR(time);
    }

    public static void Trigger(float time, float intensityFactor) {
        if (lightning == null) return;
        lightning.RunTriggerCR(time, intensityFactor);
    }

    public static void Trigger(float time, float intensityFactor, Color color) {
        if (lightning == null) return;
        lightning.RunTriggerCR(time, intensityFactor, color);
    }

    void RunTriggerCR() {
        RunTriggerCR(dTime, 1, dColor);
    }

    void RunTriggerCR(float time) {
        RunTriggerCR(time, 1, dColor);
    }

    void RunTriggerCR(float time, float intensityFactor) {
        RunTriggerCR(time, intensityFactor, dColor);
    }

    // since static function can't use StartCoroutine, I need to have its singleton do the job
    void RunTriggerCR(float time, float intensityFactor, Color color) {
        StartCoroutine(TriggerCR(time, intensityFactor, color));
    }

    IEnumerator TriggerCR(float time, float intensityFactor, Color color) {
        // set intensity and sync with boss
        light.intensity = dIntensity * intensityFactor;
        EnemyBoss.SyncLightning(light.intensity/(dIntensity*intensityFactor));

        light.color = color;
        yield return new WaitForSeconds(time);
        light.color = dColor;

        // TEST FADING INTENSITY
        StartCoroutine(FadeLight(intensityFactor));
        //light.intensity = 0;
    }

    IEnumerator FadeLight(float intensityFactor, float fadeSpeed = .3f)
    {
        while(light.intensity > 0)
        {
            light.intensity -= fadeSpeed;
            EnemyBoss.SyncLightning(light.intensity/(dIntensity*intensityFactor));
            yield return new WaitForSeconds(.1f);
        }
        light.intensity = 0;
    }


}
