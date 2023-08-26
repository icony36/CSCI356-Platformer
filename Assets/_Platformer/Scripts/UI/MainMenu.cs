using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class MainMenu : MonoBehaviour
{
    [SerializeField] private SettingsMenu settingsMenu;
    [SerializeField] private string gameLevel;

    public void PlayGame()
    {
        SceneManager.LoadScene(gameLevel);
    }

    public void OpenSettingsMenu()
    {
        settingsMenu.OpenSettingsMenu();
    }

    public void QuitGame()
    {
        Debug.Log("Quit Game");
        
        Application.Quit();
    }
}
