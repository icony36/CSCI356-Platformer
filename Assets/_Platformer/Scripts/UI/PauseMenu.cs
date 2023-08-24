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

}
