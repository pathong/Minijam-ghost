using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;

public class ProgressUI : MonoBehaviour
{
    [SerializeField] private TMP_Text progressText; 
    [SerializeField] private TMP_Text TitleText; 



    public void SetData(string des,  int curr, int max)
    {
        TitleText.text = des;
        progressText.text = string.Concat(curr, "/",max);
    }
}
