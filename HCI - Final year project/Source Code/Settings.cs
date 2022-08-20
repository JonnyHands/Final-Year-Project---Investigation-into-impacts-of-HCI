using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Audio;
using UnityEngine.UI;

public class Settings : MonoBehaviour
{
    public AudioMixer audioMixer;
    Resolution[] resolutions;
    public Dropdown resolutionDropdown;
    private void Start()
    {
        
    }
    // method to set volume
    public void SetVolume (float volume)
    {
        audioMixer.SetFloat("volume", volume);
    }

    //method to set quality
    public void SetQuality(int index)
    {
        QualitySettings.SetQualityLevel(index);
    }

    //method to toggle screen
    public void ToggleFullscreen(bool isFullscreen)
    {
        Screen.fullScreen = isFullscreen;
    }
    //method for return button to resume from previous state
    public void Return()
    {
        ToggleSettings.instance.OpenSettings(true);
        if (LevelManager.instance.isPaused)
        {
            LevelManager.instance.PauseResume();
        }
    }

    //method for return button on mainmenu
    public void ReturnMenu()
    {
        ToggleSettings.instance.MenuSettings(true);
        MainMenuHandler.instance.delete.SetActive(false);
        MainMenuHandler.instance.dialog.SetActive(false);
    }
}
