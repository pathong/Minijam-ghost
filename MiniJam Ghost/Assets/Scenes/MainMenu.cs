using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;
using TMPro;
using UnityEngine.UI;

public class MainMenu : MonoBehaviour
{
    public TMP_Text text;
    Color tCol;
    float tAlpha = 0;
    public TMP_Text next;
    Color nCol;
    float nAlpha = 0;

    float timer = 0;

    public Image img;

    void Start() {
        tCol = text.color;
        text.color = new Color(tCol.r, tCol.g, tCol.b, 0);
        nCol = text.color;
        next.color = new Color(nCol.r, nCol.g, nCol.b, 0);
    }

    void Update() {
        timer += Time.deltaTime;

        if (Input.GetKeyDown(KeyCode.Return)) {
            StartCoroutine(GoNextScene("Tutorial"));
        }
        
        // if (timer > 1 && tAlpha < 1) {
        //     tAlpha += 0.7f * Time.deltaTime;
        //     if (tAlpha > 1) tAlpha = 1;
        //     text.color = new Color(tCol.r, tCol.g, tCol.b, tAlpha);
        // }

        if (timer > 2.2f && nAlpha < 1) {
            nAlpha += 0.7f * Time.deltaTime;
            if (nAlpha > 1) nAlpha = 1;
            next.color = new Color(nCol.r, nCol.g, nCol.b, nAlpha);
        }

        if (timer > 4f) {
            nAlpha = (Mathf.Sign(timer*5) / 4) + 0.75f;
            next.color = new Color(nCol.r, nCol.g, nCol.b, nAlpha);
        }
    }

    IEnumerator GoNextScene(string scene) {
        img.enabled = true;
        StartCoroutine(Fade());
        yield return new WaitForSeconds(1);
        SceneManager.LoadScene(scene);
    }

    IEnumerator Fade() {
        float a = 0;
        img.color = new Color(0,0,0,0);

        while (true) {
            img.color = new Color(0,0,0,a);
            a += 0.2f;
            yield return new WaitForSeconds(0.1f);
            if (a > 1) break;
        }
    }

    public void Button1() {
        StartCoroutine(GoNextScene("Night1"));
    }
    public void Button2() {
        StartCoroutine(GoNextScene("Night2"));
    }
    public void Button3() {
        StartCoroutine(GoNextScene("Night4"));
    }
    public void Button4() {
        StartCoroutine(GoNextScene("Night5"));
    }
    public void Quit() {
        Application.Quit();
    }
}
