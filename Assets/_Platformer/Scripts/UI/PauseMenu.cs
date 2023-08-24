using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.UI;

public class PauseMenu : MonoBehaviour
{

    public static bool GameIsPaused = false;
    public string mainMenuScene;
    public GameObject pauseMenuUI;
    public GameObject settingMenuUI;
    public Slider musicSlider;
    public AudioSource music;
    public Slider sfxSlider;
    public AudioSource[] sfx;
    public Dropdown resolutionDropdown;


    private const string MusicVolumeKey = "MusicVolume";
    private const string SFXVolumeKey = "SFXVolume";

    // Start is called before the first frame update
    void Start()
    {

        // Load saved volume settings or use default values
        float savedMusicVolume = PlayerPrefs.GetFloat(MusicVolumeKey, 0.5f);
        float savedSFXVolume = PlayerPrefs.GetFloat(SFXVolumeKey, 0.5f);

        music.volume = savedMusicVolume;
        musicSlider.value = savedMusicVolume;

        foreach (AudioSource sfxSource in sfx)
        {
            sfxSource.volume = savedSFXVolume;
        }
        sfxSlider.value = savedSFXVolume;

        int currentResolutionIndex = GetCurrentResolutionIndex();
        resolutionDropdown.value = currentResolutionIndex;
        resolutionDropdown.RefreshShownValue();

        List<string> resolutionOptions = new List<string>();
        foreach (Resolution resolution in Screen.resolutions)
        {
            resolutionOptions.Add($"{resolution.width} x {resolution.height}");
        }
        resolutionDropdown.ClearOptions();
        resolutionDropdown.AddOptions(resolutionOptions);

    }

    // Update is called once per frame
    void Update()
    {
        if (Input.GetKeyDown(KeyCode.Escape))
        {
            if (GameIsPaused)
            {
                Resume();
            }
            else
            {
                Pause();
            }
        }
    }

    public void Resume()
    {
        pauseMenuUI.SetActive(false);
        Time.timeScale = 1f;
        GameIsPaused = false;
    }

    public void Pause()
    {
        pauseMenuUI.SetActive(true);
        Time.timeScale = 0f;
        GameIsPaused= true;
    }

    public void MainMenu()
    {
        Time.timeScale = 1f;
        SceneManager.LoadScene(mainMenuScene);
    }

    public void Setting()
    {
        pauseMenuUI.SetActive(false);
        settingMenuUI.SetActive(true);
    }

    public void Back()
    {
        settingMenuUI.SetActive(false);
        pauseMenuUI.SetActive(true);
    }

    public void RestartGame()
    {
        Time.timeScale = 1f;
        SceneManager.LoadScene(SceneManager.GetActiveScene().buildIndex);
        print("Restart");
    }

    public void MusicOnVolumeChanged()
    {
        music.volume = musicSlider.value;
        // Save the updated music volume setting
        PlayerPrefs.SetFloat(MusicVolumeKey, musicSlider.value);
        PlayerPrefs.Save();
    }

    public void SFXOnVolumeChanged()
    {
        foreach (AudioSource sfxSource in sfx)
        {
            sfxSource.volume = sfxSlider.value;
        }
        // Save the updated SFX volume setting
        PlayerPrefs.SetFloat(SFXVolumeKey, sfxSlider.value);
        PlayerPrefs.Save();
    }

    private void InitializeResolutionDropdown()
    {
        resolutionDropdown.ClearOptions();

        List<string> resolutionOptions = new List<string>();

        for (int i = 0; i < Screen.resolutions.Length; i++)
        {
            Resolution resolution = Screen.resolutions[i];
            string option = $"{resolution.width} x {resolution.height}";
            resolutionOptions.Add(option);
        }

        resolutionDropdown.AddOptions(resolutionOptions);
    }

    public void SetResolution(int resolutionIndex)
    {
        Resolution resolution = Screen.resolutions[resolutionIndex];
        Screen.SetResolution(resolution.width, resolution.height, Screen.fullScreen);
    }

    public void OnResolutionChanged(int resolutionIndex)
    {
        if (resolutionIndex >= 0 && resolutionIndex < Screen.resolutions.Length)
        {
            Resolution resolution = Screen.resolutions[resolutionIndex];
            Screen.SetResolution(resolution.width, resolution.height, Screen.fullScreen);
        }
    }

    private int GetCurrentResolutionIndex()
    {
        Resolution currentResolution = Screen.currentResolution;
        for (int i = 0; i < Screen.resolutions.Length; i++)
        {
            if (Screen.resolutions[i].width == currentResolution.width &&
                Screen.resolutions[i].height == currentResolution.height)
            {
                return i;
            }
        }
        return 0; // Default to the first resolution if current resolution is not found
    }
}
