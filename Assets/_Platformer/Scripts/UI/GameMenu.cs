using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.EventSystems;
using UnityEngine.InputSystem;
using UnityEngine.UI;
using static Player;

public class GameMenu : GenericSingleton<GameMenu>
{
    [SerializeField] private SettingsMenu settingsMenu;
    [SerializeField] private GameObject pauseMenu;
    [SerializeField] private GameObject gameOverMenu;
    [SerializeField] private GameObject gameWinMenu;

    private GameManager gameManager;

    private enum GameUIState
    {
        GamePlay,
        GamePause,
        GameOver,
        GameIsFinished
    }

    private GameUIState currentState;

    private void Start()
    {
        gameManager = GameManager.Instance.GetComponent<GameManager>();
        
        SwitchUIState(GameUIState.GamePlay);
    }

    private void SwitchUIState(GameUIState state)
    {
        settingsMenu.CloseSettingsMenu();
        pauseMenu.SetActive(false);
        gameOverMenu.SetActive(false);
        gameWinMenu.SetActive(false);

        Time.timeScale = 1; // to prevent game stop

        switch (state)
        {
            case GameUIState.GamePlay:
                break;
            case GameUIState.GamePause:
                Time.timeScale = 0;
                Cursor.visible = true;
                pauseMenu.SetActive(true);
                break;
            case GameUIState.GameOver:
                Cursor.visible = true;
                gameOverMenu.SetActive(true);
                break;
            case GameUIState.GameIsFinished:
                Cursor.visible = true;
                gameWinMenu.SetActive(true);
                break;
            default:
                break;
        }

        currentState = state;
    }

    public void TogglePauseMenu()
    {
        if (currentState == GameUIState.GamePlay)
        {
            SwitchUIState(GameUIState.GamePause);
        }
        else if (currentState == GameUIState.GamePause)
        {
            SwitchUIState(GameUIState.GamePlay);
        }

        EventSystem.current.SetSelectedGameObject(null);
    }

    public void GoToMainMenu()
    {
        gameManager.ReturnToMainMenu();
    }

    public void OpenSettingsMenu()
    {
        settingsMenu.OpenSettingsMenu();
    }

    public void CloseSettingsMenu()
    {
        settingsMenu.CloseSettingsMenu();
    }

    public void RestartGame()
    {
        gameManager.RestartGame();
    }

    public void ShowGameOverMenu()
    {
        SwitchUIState(GameUIState.GameOver);
    }

    public void ShowGameWinMenu()
    {
        SwitchUIState(GameUIState.GameIsFinished);
    }
}
