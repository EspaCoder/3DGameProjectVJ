using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI; 
using UnityEngine.Audio; 

public class optionsMenuController : MonoBehaviour {

    public AudioMixer audioMixer;
    public Text perc;

    public Dropdown resolutionDropdown;
    public Dropdown controllerDropdown; 

    Resolution[] resolutions;

    void Start()
    {
        resolutions = Screen.resolutions;

        resolutionDropdown.ClearOptions();

        List<string> stringoptions = new List<string>();

        int currentResolutionIndex = 0; 
        for (int i = 0; i < resolutions.Length; ++i)
        {
            string nextop = resolutions[i].width + "x" + resolutions[i].height;
            stringoptions.Add(nextop); 

            if (resolutions[i].width == Screen.currentResolution.width && resolutions[i].height == Screen.currentResolution.height)
            {
                currentResolutionIndex = i; 
            }
        }

        resolutionDropdown.AddOptions(stringoptions);
        resolutionDropdown.value = currentResolutionIndex;
        resolutionDropdown.RefreshShownValue();    
    } 

    public void setVolume (float volume)
    {
        audioMixer.SetFloat("volume", volume);
        int aux = Mathf.RoundToInt( ((volume + 80.0f) / 80.0f)*100.0f);
        perc.text = aux + " %"; 
    }

    public void setFullscreen (bool isFullscreen)
    {
        Screen.fullScreen = isFullscreen; 
    }

    public void setController (int controllerIndex)
    {
        if (controllerIndex == 0) variableScript.mouseEnabled = true;
        else variableScript.mouseEnabled = false; 
    }

    public void setResolution (int resolutionIndex)
    {
        Resolution resolution = resolutions[resolutionIndex];
        Screen.SetResolution(resolution.width, resolution.height, Screen.fullScreen); 
    }
}
