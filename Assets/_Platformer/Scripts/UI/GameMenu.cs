using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.EventSystems;
using UnityEngine.InputSystem;
using UnityEngine.UI;
using static Player;

public class GameMenu : GenericSingleton<GameMenu>
{
    [SerializeField] private GameObject PauseMenu;
    [SerializeField] private GameObject GameOverMenu;
    [SerializeField] private GameObject GameWinMenu;

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
        PauseMenu.SetActive(false);
        GameOverMenu.SetActive(false);
        GameWinMenu.SetActive(false);

        Time.timeScale = 1; // to prevent game stop

        switch (state)
        {
            case GameUIState.GamePlay:
                break;
            case GameUIState.GamePause:
                Time.timeScale = 0;
                PauseMenu.SetActive(true);
                break;
            case GameUIState.GameOver:
                GameOverMenu.SetActive(true);
                break;
            case GameUIState.GameIsFinished:
                GameWinMenu.SetActive(true);
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
