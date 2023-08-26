using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using System.Linq;

public class SettingsMenu : MonoBehaviour
{
    [SerializeField] private GameObject settingMenuUI;
    [SerializeField] private AudioSource bgm;
    [SerializeField] private Slider bgmSlider;
    [SerializeField] private AudioSource sfx;
    [SerializeField] private Slider sfxSlider;
    [SerializeField] private Dropdown resolutionDropdown;

    private const string BGM_VOLUME = "BGMVolume";
    private const string SFX_VOLUME = "SFXVolume";

    // Resolution
    private List<Resolution> resolutionList;

    // Start is called before the first frame update
    void Start()
    {
        settingMenuUI.SetActive(false);

        // Load saved volume settings or use default values
        float savedMusicVolume = PlayerPrefs.GetFloat(BGM_VOLUME, 0.5f);
        float savedSFXVolume = PlayerPrefs.GetFloat(SFX_VOLUME, 0.5f);

        bgm.volume = savedMusicVolume;
        bgmSlider.value = savedMusicVolume;

        sfx.volume = savedSFXVolume;
        sfxSlider.value = savedSFXVolume;

        
        InitResolutionDropdown();
        SetCurrentResolution();
    }

    public void OpenSettingsMenu()
    {
        settingMenuUI.SetActive(true);
    }

    public void CloseSettingsMenu()
    {
        settingMenuUI.SetActive(false);
    }

    public void OnBGMVolumeChanged()
    {
        bgm.volume = bgmSlider.value;

        // Save the updated BGM volume
        PlayerPrefs.SetFloat(BGM_VOLUME, bgm.volume);
        PlayerPrefs.Save();
    }

    public void OnSFXVolumeChanged()
    {
        sfx.volume = sfxSlider.value;

        // Save the updated SFX volume 
        PlayerPrefs.SetFloat(SFX_VOLUME, sfx.volume);
        PlayerPrefs.Save();
    }

    private void InitResolutionDropdown()
    {
        resolutionDropdown.ClearOptions();

        List<string> resolutionOptions = new List<string>();

        resolutionList = Screen.resolutions.Where(r => r.refreshRateRatio.Equals(Screen.currentResolution.refreshRateRatio)).ToList();

        for (int i = 0; i < resolutionList.Count; i++)
        {
            Resolution resolution = resolutionList[i];
            string option = $"{resolution.width} x {resolution.height}";
            resolutionOptions.Add(option);
        }

        resolutionDropdown.AddOptions(resolutionOptions);
    }

    private void SetCurrentResolution()
    {
        int currentResolutionIndex = 0;

        Resolution currentResolution = Screen.currentResolution;
        for (int i = 0; i < resolutionList.Count; i++)
        {
            if (resolutionList[i].width == currentResolution.width && resolutionList[i].height == currentResolution.height)
            {
                currentResolutionIndex = i;
            }
        }

        resolutionDropdown.value = currentResolutionIndex;
        resolutionDropdown.RefreshShownValue();
    }

    public void SetResolution(int resolutionIndex)
    {
        Resolution resolution = resolutionList[resolutionIndex];
        Screen.SetResolution(resolution.width, resolution.height, Screen.fullScreen);
    }
}
