using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class HealthBarManager : MonoBehaviour
{
    [SerializeField] private HeartBPSBar bar;
    [SerializeField] private HealthBarData[] datas;
    private HealthBarData data;
    [SerializeField] private PlayerHealth playerHealth;

    // have 4 health
    private void Update()
    {
        data = datas[playerHealth.currentHealth];

        bar.myLineRenderer.startColor = data.color;
        bar.myLineRenderer.endColor = data.color;
        bar.amplitude = data.amp;

    }


}


[System.Serializable]
class HealthBarData
{
    public Color color;
    public float amp;

}
