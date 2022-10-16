using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;

public class ProgressUI : MonoBehaviour
{
    [SerializeField] private ProgressManager progressManager;
    [SerializeField] private TMP_Text progressText; 
    [SerializeField] private TMP_Text TitleText; 

    private void Awake()
    {
        if(progressManager == null) { progressManager = ProgressManager.i; }
        progressManager.OnProgressActivate += SetData;
    }


    private void Start()
    {
        SetData();
    }
    public void SetData()
    {
        TitleText.text = progressManager.QuestName;
        progressText.text = string.Concat(progressManager.currentProgress, "/", progressManager.progress);
    }
}
