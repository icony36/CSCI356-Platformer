using System.Collections;
using System.Collections.Generic;
using UnityEditor;
using UnityEngine;
using UnityEngine.InputSystem;
using UnityEngine.SceneManagement;

public class GameManager : GenericSingleton<GameManager>
{
    [SerializeField] GameMenu gameMenu;

    public Player Player { get; private set; }

    public PlayerData playerData;
    public PlayerData initData;
    public Dictionary<int, bool> enemyState = new Dictionary<int, bool>();
    public Dictionary<int, bool> powerUpState = new Dictionary<int, bool>();

    [SerializeField] private GameObject enemiesHolder;
    [SerializeField] private GameObject powerupHolder;

    private bool isGameOver;

    private void Start()
    {
        Player = GameObject.FindWithTag("Player").GetComponent<Player>();
        Init();
    }

    private void Update()
    {
        HandleUIInput();

        if (!isGameOver)
        {
            if(Player.CurrentState == Player.PlayerState.Dead)
            {
                isGameOver = true;
                GameIsOver();
            }
        }
    }

    private void HandleUIInput()
    {
        if (Input.GetKeyDown(KeyCode.Escape))
        {
            gameMenu.TogglePauseMenu();
        }
    }

    private void GameIsOver()
    {
        Debug.Log("GAME OVER");
        gameMenu.ShowGameOverMenu();
    }

    public void GameIsFinished()
    {
        Debug.Log("GAME FINISHED");
        Player.DisableAllActions();
        Player.PlayAnimVictory();
        gameMenu.ShowGameWinMenu();
    }

    public void ReturnToMainMenu()
    {
        Time.timeScale = 1.0f;

        SceneManager.LoadScene("MainMenu");
    }

    public void RestartGame()
    {
        SceneManager.LoadScene(SceneManager.GetActiveScene().name);
    }

    public void AssignEnemyID() //used in editor
    {

    }

    public void AssignPowerupID() //used in editor
    {
        int i = 0;

        foreach (Transform child in powerupHolder.transform)
        {
            child.gameObject.GetComponent<Powerup>().ID = i;
            powerUpState.Add(child.gameObject.GetComponent<Powerup>().ID, false);
            EditorUtility.SetDirty(child.gameObject.GetComponent<Powerup>());
            i++;
        }
    }

    public void Init() //called on new game start
    {
        foreach (Transform child in powerupHolder.transform)
        {
            if(child.gameObject.GetComponent<Powerup>())
                powerUpState.Add(child.gameObject.GetComponent<Powerup>().ID, false);
        }   
    }

    public void SaveGame()
    {
        SaveData savedData = new SaveData
        {
            currentHealth = playerData.currentHealth,
            
        };

        string filePath = Application.persistentDataPath + "/savedata.sav";

        DataSerializer.SaveJson(savedData, filePath);

        Debug.Log("Game saved");
    }

    public void LoadGame()
    {

    }
}
