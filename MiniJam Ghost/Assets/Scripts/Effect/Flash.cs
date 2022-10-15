using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Experimental.Rendering.Universal;

[RequireComponent(typeof(Light2D))]
public class Flash : MonoBehaviour
{
    // makeshift singleton
    static Flash flash;

    public Color dColor;
    public float dTime;
    public float dIntensity;
    //public float fadeSpeed;

    // reference
    new Light2D light;

    void Awake() {
        // singleton
        if (flash == null) flash = this;

        light = GetComponent<Light2D>();
    }

    void Start() {
        light.intensity = 0;;
    }

    public static void Trigger() {
        if (flash == null) return;
        flash.RunTriggerCR();
    }

    public static void Trigger(float time) {
        if (flash == null) return;
        flash.RunTriggerCR(time);
    }

    public static void Trigger(float time, float intensityFactor) {
        if (flash == null) return;
        flash.RunTriggerCR(time, intensityFactor);
    }

    public static void Trigger(float time, float intensityFactor, Color color) {
        if (flash == null) return;
        flash.RunTriggerCR(time, intensityFactor, color);
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
        light.intensity = dIntensity * intensityFactor;
        light.color = color;
        yield return new WaitForSeconds(time);
        light.color = dColor;

        // TEST FADING INTENSITY
        StartCoroutine(FadeLight());
        //light.intensity = 0;
    }

    IEnumerator FadeLight(float fadeSpeed = .3f)
    {
        while(light.intensity > 0)
        {
            light.intensity -= fadeSpeed;
            Debug.Log(light.intensity);
            yield return new WaitForSeconds(.3f);
        }
    }


}
