using DG.Tweening;
using System.Collections;
using System.Collections.Generic;
using System.Threading.Tasks;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.Rendering.PostProcessing;
using UnityEngine.SceneManagement;
using static DG.Tweening.DOCurve;
using TMPro;

public class MainMenu : MonoBehaviour
{
    private Camera m_Camera;

    public GameObject loadingScreen;
    public Image loadingBar;

    public TextMeshProUGUI textMeshPro;

    public BlurBackground blurBackground;

    private Vector3[] list = new Vector3[]
    {
       
        new Vector3(0, 12, -15)
    };
    



    public async void PlayGame() 
    {

        PauseMenu.quitGame = false;

       
        m_Camera = Camera.main;

       


        m_Camera.transform.DOLookAt(new Vector3(0, 0, 0), 0.5f);
        await Task.Delay(1000);
       
        m_Camera.transform.DOPath(list, 1.5f, PathType.Linear, PathMode.Full3D).SetLookAt(new Vector3(0, 0, 0), true);
        

        blurBackground.Blur();
        await Task.Delay(2000);
        StartCoroutine(LoadSceneAsync(1));
        

       


     
     
    }


    IEnumerator LoadSceneAsync(int sceneNumber) 
    {
        AsyncOperation operation = SceneManager.LoadSceneAsync(sceneNumber);
        loadingScreen.SetActive(true);

        while (!operation.isDone) 
        {
            float progressValue = Mathf.Clamp01(operation.progress/0.9f);
            //loadingBar.fillAmount = progressValue;
            textMeshPro.text = (operation.progress / 0.9f * 100).ToString() + "%";
            yield return null;
        }
    }


    public void QuitGame() 
    {
        Application.Quit();
    }


   

}
