using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System;
using UnityEngine.UI;
using System.Threading.Tasks;
using TMPro;

public class ChessUIManager : MonoBehaviour
{
    [SerializeField] private GameObject UIParent;
    [SerializeField] private TextMeshProUGUI resultText;

    public void HideUI() 
    {
        UIParent.SetActive(false);
    }

    public async void OnGameFinished(string winner) 
    {
        await Task.Delay(1000);
        UIParent.SetActive(true);
        resultText.text = string.Format("{0} won!", winner);
    }
}
