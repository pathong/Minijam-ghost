using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;
using UnityEngine.SceneManagement;
using UnityEngine.UI;

public class Tutotial : MonoBehaviour
{
    private int progress = 0;
    public TMP_Text t1, t2, t3, t4, t5;
    public Image img;

    void Start() {
        t1.enabled = false;
        t2.enabled = false;
        t3.enabled = false;
        t4.enabled = false;
        t5.enabled = false;
        img.enabled = false;

        InputManager.Controls.Enable();
    }
    
    void Update() {
        if (progress == 0) {
            t1.enabled = true;
            if (Input.GetKeyDown(KeyCode.E)) {
                progress++;
                t1.enabled = false;
            };
        }
        if (progress == 1) {
            t2.enabled = true;
            if (Input.GetKeyDown(KeyCode.S)) {
                progress++;
                t2.enabled = false;
            };
        }
        if (progress == 2) {
            t3.enabled = true;
            if (Input.GetMouseButtonDown(0)) {
                progress++;
                t3.enabled = false;
            };
        }
        if (progress == 3) {
            t4.enabled = true;
            if (Input.GetKeyDown(KeyCode.R)) {
                progress++;
                t4.enabled = false;
            };
        }
        if (progress == 4) {
            t5.enabled = true;
            if (Input.GetKeyDown(KeyCode.Return)) {
                progress++;
                t5.enabled = false;
                StartCoroutine(GoNextScene());
            };
        }

    }

    IEnumerator GoNextScene() {
        img.enabled = true;
        StartCoroutine(Fade());
        yield return new WaitForSeconds(1);
        SceneManager.LoadScene("Night1");
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
}
