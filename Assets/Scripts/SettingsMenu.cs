using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.Audio;

public class SettingsMenu : MonoBehaviour
{

    public AudioMixer audioMixer;
    public TMPro.TMP_Dropdown resolutionDropdown;

    Resolution[] resolutions;
    List<Resolution> filteredResolutions;

    private double currentRefreshRate;
    private void Start()
    {
        resolutions = Screen.resolutions;
        filteredResolutions = new List<Resolution>();

        resolutionDropdown.ClearOptions();
        currentRefreshRate = Screen.currentResolution.refreshRateRatio.value;

        for (int i = 0; i < resolutions.Length; i++)
        {
            if (resolutions[i].refreshRateRatio.value >= 60) 
            {
                filteredResolutions.Add(resolutions[i]);
            }
            
            
        }
      

        List<string> options = new List<string>();

        int index = 0;
       for(int i = 0; i < filteredResolutions.Count;i++)
        {
            if (filteredResolutions[i].refreshRateRatio.value >= 60)
            {
                string option = filteredResolutions[i].width + " x " + filteredResolutions[i].height + "\n @ " + filteredResolutions[i].refreshRateRatio.value + "hz";
                
                options.Add(option);
                if (filteredResolutions[i].width == Screen.currentResolution.width && filteredResolutions[i].height == Screen.currentResolution.height && Screen.currentResolution.refreshRateRatio.value == filteredResolutions[i].refreshRateRatio.value)
                {
                    index = i;
                }
            }
            
        }

        resolutionDropdown.AddOptions(options);

        resolutionDropdown.value = index;
        resolutionDropdown.RefreshShownValue();
    }
    public void SetVolume(float volume) 
    {
        audioMixer.SetFloat("Volume", volume);
    }

    public void SetQuality(int qualityIndex) 
    {
        QualitySettings.SetQualityLevel(qualityIndex);
    }

    public void SetFullscreen(bool isFullscreen) 
    {
        Screen.fullScreen = isFullscreen;
    }

    public void SetResolution(int resolutionIndex) 
    {
        Resolution resolution = filteredResolutions[resolutionIndex];
        FullScreenMode fullScreenMode;

        if (Screen.fullScreen) 
            fullScreenMode = FullScreenMode.ExclusiveFullScreen;
        else 
            fullScreenMode = FullScreenMode.Windowed;

        Screen.SetResolution(resolution.width, resolution.height, fullScreenMode, resolution.refreshRateRatio);
        
    }
}
