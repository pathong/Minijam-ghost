using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;

[RequireComponent(typeof(TextMeshPro))]
public class Countdown : MonoBehaviour
{
    TextMeshPro tmp;
    public float setTime = 5f;
    private float time = 0;
    public bool start = false;
    float count = 0;

    private void Start()
    {
        tmp = gameObject.GetComponent<TextMeshPro>();
        time = setTime;
    }

    private void Update()
    {
        if (start)
            CountdownOne();
    }

    public void CountdownOne()
    {
        time -= Time.deltaTime;
        if (time > 0)
        {
            tmp.color = new Color(255, 255, 255, 0.5f);
            tmp.text = Mathf.RoundToInt(time).ToString();
        }
        else if (count < 2)
        {
            tmp.text = "Need Fixing";
            tmp.color = new Color(255, 0, 0, 0.5f);
            time = setTime;
            start = false;
            count++;
        }
        else
        {
            tmp.text = "Running";
            tmp.color = new Color(0, 255, 0, 0.5f);
            time = setTime;
            start = false;
        }
    }
}
