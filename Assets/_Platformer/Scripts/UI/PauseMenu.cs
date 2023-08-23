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


    // Start is called before the first frame update
    void Start()
    {
        
        musicSlider.value = music.volume;
        sfxSlider.value = sfx[0].volume;
        
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
    }

    public void SFXOnVolumeChanged()
    {
        foreach (AudioSource sfx in sfx)
        {
            sfx.volume = sfxSlider.value;
        }
    }

}
