using DG.Tweening;
using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System.Threading.Tasks;
using TMPro;
using UnityEngine.UI;
using UnityEngine.SceneManagement;

public class PauseMenu : MonoBehaviour
{
    public static bool GameIsPaused = false;

    public static bool quitGame;


    public GameObject pauseMenuUI;
    public Camera m_camera;
    public GameObject optionsMenuUI;

    public GameObject loadingScreen;
    public Image loadingBar;
    public TextMeshProUGUI textPercent;


    private Vector3[] list = new Vector3[]
    {

        new Vector3(40,22,-37)
    };

    void Update()
    {
        if (Input.GetKeyDown(KeyCode.Escape)) 
        {
            if (GameIsPaused)
            {
                
                Resume();
                //if (optionsMenuUI.activeInHierarchy) optionsMenuUI.SetActive(false);
            }
            else 
            {
                Pause();
            }
        }
    }

    private void Pause()
    {
        pauseMenuUI.SetActive(true);
        Time.timeScale = 0;
        GameIsPaused = true;
    }

    public void Resume()
    {
        pauseMenuUI.SetActive(false);
        Time.timeScale = 1f;
        GameIsPaused = false;
    }

    public void LoadMenu() 
    {
        optionsMenuUI.SetActive(true);
    }

    public async void QuitGame()
    {
        Time.timeScale = 1f;
        GameIsPaused = false;
        quitGame = true;
       

        m_camera.transform.DOPath(list, 1.5f, PathType.Linear, PathMode.Full3D).SetLookAt(new Vector3(0, 0, 0), true);
        await Task.Delay(1500);
        m_camera.transform.DORotate(new Vector3(9,-27,0), 0.5f);
        await Task.Delay(600);
        StartCoroutine(LoadSceneAsync(0));

    }


    IEnumerator LoadSceneAsync(int sceneNumber)
    {
        AsyncOperation operation = SceneManager.LoadSceneAsync(sceneNumber);
        loadingScreen.SetActive(true);

        while (!operation.isDone)
        {
            float progressValue = Mathf.Clamp01(operation.progress / 0.9f);
            //loadingBar.fillAmount = progressValue;
            textPercent.text = (operation.progress / 0.9f * 100).ToString() + "%";
            yield return null;
        }
    }
}
