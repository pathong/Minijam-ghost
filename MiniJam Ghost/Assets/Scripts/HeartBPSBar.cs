using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class HeartBPSBar : MonoBehaviour
{
    public LineRenderer myLineRenderer;
    public int points;
    public float amplitude = 1;
    public float frequency = 1;
    public Vector2 xLimits = new Vector2(0, 1);
    public float movementSpeed = 1;
    [Range(0, 2 * Mathf.PI)]
    public float radians;
    public float dRainAmp = .001f;
    void Start()
    {
        myLineRenderer = GetComponent<LineRenderer>();
    }

    void Draw()
    {
        float Tau = 2 * Mathf.PI;
        float xStart = xLimits.x * Tau;
        float xFinish = xLimits.y * Tau;

        myLineRenderer.positionCount = points;
        for (int currentPoint = 0; currentPoint < points; currentPoint++)
        {
            float progress = (float)currentPoint / (points - 1);
            float x = Mathf.Lerp(xStart, xFinish, progress);

            float _x = x * frequency + Time.timeSinceLevelLoad;
            float y = amplitude * 20 * (
            Mathf.Pow(Mathf.Sin(_x),63) * Mathf.Sin((_x + 1.5f)) 
            + .02f * Mathf.Pow(Mathf.Sin((_x + 0.9f)), 60)
            );

            //float y = amplitude * 2 * Mathf.Sin(x + Time.timeSinceLevelLoad);

            myLineRenderer.SetPosition(currentPoint, new Vector3(x, y, 0));
        }
    }

    void Update()
    {
        Draw();
    }
}
