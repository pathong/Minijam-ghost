using System.Collections; using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.UI;
using UnityEngine.InputSystem;

public class InGameMenu : MonoBehaviour
{
    public static InGameMenu instance;
    public Image blackPanel;
    public GameObject PauseScreen;
    public GameObject DeadScreen;
    public GameObject FinishScreen;
    private bool isPausing;
    private bool isDead;
    private bool isFinish;

        

    private void Awake()
    {
        LeanTween.reset();
        isPausing = false;
        isDead = false;
        isFinish = false;
        if(instance == null) { instance = this; }
        else { Destroy(instance); }
    }

    private void Start()
    {
        OpenBlackPanel();
    } 


    private void Update()
    {
        if (Keyboard.current.escapeKey.wasPressedThisFrame && !isDead && !isFinish)
        {
            if (isPausing) Continue();
            else if(!isPausing) Pause();
        }
    }
    public void Pause()
    {
        isPausing = true;
        PauseScreen.SetActive(true);
        InputManager.Controls.Disable();
        Time.timeScale = 0;
    }
    public void Continue()
    {
        isPausing = false;
        PauseScreen.SetActive(false);
        InputManager.Controls.Enable();
        Time.timeScale = 1;
    }

    public void ReStart()
    {
        LeanTween.value(gameObject, 0, 1, 2f).setIgnoreTimeScale(true).setOnUpdate((float val) =>
        {
            Color c = blackPanel.color;
            c.a = val;
            blackPanel.color = c;
        }).setOnComplete(() => {
            Time.timeScale = 1;
            InputManager.Controls.Enable();
            SceneManager.LoadScene(SceneManager.GetActiveScene().name);
            }
        );
    }

    public void BackToMenu()
    {
        LeanTween.value(gameObject, 0, 1, 2f).setIgnoreTimeScale(true).setOnUpdate((float val) =>
        {
            Color c = blackPanel.color;
            c.a = val;
            blackPanel.color = c;
        }).setOnComplete(() => {
            Time.timeScale = 1;
            InputManager.Controls.Enable();
            SceneManager.LoadScene("MainMenu");
            }
        );

    }
    public void NextScene(string sceneName)
    {
        LeanTween.value(gameObject, 0, 1, 2f).setIgnoreTimeScale(true).setOnUpdate((float val) =>
        {
            Color c = blackPanel.color;
            c.a = val;
            blackPanel.color = c;
        }).setOnComplete(() => {
            Time.timeScale = 1;
            InputManager.Controls.Enable();
            SceneManager.LoadScene(sceneName);
            }
        );
    }


    public void OnPlayerDeadHanddler()
    {
        DeadScreen.SetActive(true);
        isDead = true;
        InputManager.Controls.Disable();
        Time.timeScale = 0;
    }

    public void OnPlayerFinishHanddler()
    {
        FinishScreen.SetActive(true);
        isFinish = true;
        InputManager.Controls.Disable();
        Time.timeScale = 0;
    }



    public void OpenBlackPanel()
    {
        
        LeanTween.value(gameObject, 1, 0, 2f).setIgnoreTimeScale(true).setOnUpdate((float val) =>
        {
            Color c = blackPanel.color;
            c.a = val;
            blackPanel.color = c; 
        }).setOnComplete(()=> InputManager.Controls.Enable());
        
    }


}

