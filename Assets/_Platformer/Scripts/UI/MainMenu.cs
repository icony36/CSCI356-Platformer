using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class MainMenu : MonoBehaviour
{
    [SerializeField] private GameState gameState;
    [SerializeField] private SettingsMenu settingsMenu;
    [SerializeField] private string gameLevel;
    [SerializeField] private GameObject loadButton;

    private void Awake()
    {
        CheckIfHasSavedFile();
    }

    private void CheckIfHasSavedFile()
    {
        loadButton.SetActive(false);

        #if UNITY_STANDALONE
                string filePath = Application.streamingAssetsPath + "/savedata.sav";
        #endif

        #if UNITY_WEBGL
                string filePath = Application.persistentDataPath + "/savedata.sav";
        #endif

        if (System.IO.File.Exists(filePath))
        {
            loadButton.SetActive(true);
        }
    }


    public void PlayGame()
    {
        gameState.newGame = true;
        gameState.restartGame = false;
        SceneManager.LoadScene(gameLevel);
    }

    public void OpenSettingsMenu()
    {
        settingsMenu.OpenSettingsMenu();
    }

    public void LoadGame()
    {
        gameState.newGame = false;
        gameState.restartGame = true;
        SceneManager.LoadScene(gameLevel);
    }

    public void QuitGame()
    {
        Debug.Log("Quit Game");
        
        Application.Quit();
    }
}
